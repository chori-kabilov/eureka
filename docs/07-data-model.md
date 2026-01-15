# 07 — Data Model (DB Schema) — MVP

Документ фиксирует схему данных для MVP CRM учебного центра:
- таблицы и поля
- связи (FK)
- ограничения (unique/check)
- индексы
- правила soft delete

Стек: PostgreSQL 16, EF Core 8.

---

## 1) Общие правила

### 1.1 Базовые типы
- `uuid` для идентификаторов (PK)
- `timestamptz` для дат/времени (UTC)
- `text` для строк (где нет жёсткого лимита)
- `varchar(n)` можно использовать, но не обязательно

### 1.2 Аудит (минимальный)
Для сущностей, где это важно:
- `created_at` (timestamptz, not null)
- `created_by` (uuid, null) — id пользователя
- `updated_at` (timestamptz, null)
- `updated_by` (uuid, null)

### 1.3 Soft delete (две колонки)
Используем только:
- `is_deleted` (boolean, not null, default false)
- `deleted_at` (timestamptz, null)

Soft delete применяется только к:
- Users
- Courses
- Groups

Soft delete НЕ применяется к:
- Lessons
- AttendanceRecords
- Payments

Причина: Lessons/Attendance/Payments — исторические записи, их “удаление” ломает доверие и отчёты.
Для уроков используется статус `cancelled`.

---

## 2) Таблицы

---

## 2.1 users

Хранит всех пользователей: admin/teacher/student/parent (роль в поле `role`).

**Поля:**
- `id` uuid PK
- `role` smallint not null  
  (1=Admin, 2=Teacher, 3=Student, 4=Parent)
- `full_name` text not null
- `phone` text not null
- `email` text null
- `password_hash` text not null
- `status` smallint not null (1=Active, 2=Inactive)

**Soft delete:**
- `is_deleted` boolean not null default false
- `deleted_at` timestamptz null

**Audit:**
- `created_at` timestamptz not null
- `created_by` uuid null
- `updated_at` timestamptz null
- `updated_by` uuid null

**Ограничения:**
- `phone` уникален среди НЕ удалённых пользователей  
  (уникальный индекс по phone с условием `is_deleted=false`)

**Индексы:**
- idx_users_role (role)
- uq_users_phone_active (phone) WHERE is_deleted = false

> Примечание: если в центре бывают “общие телефоны семьи”, можно убрать уникальность и сделать поиск по phone. Но для MVP уникальность телефона резко упрощает жизнь.

---

## 2.2 parent_students (связь родитель ↔ дети)

**Поля:**
- `parent_id` uuid FK → users(id) (role=Parent)
- `student_id` uuid FK → users(id) (role=Student)
- `created_at` timestamptz not null

**PK:**
- составной PK (`parent_id`, `student_id`)

**Ограничения:**
- запрет одинаковых пар (обеспечивается PK)

**Индексы:**
- idx_parent_students_student (student_id)

---

## 2.3 courses

**Поля:**
- `id` uuid PK
- `name` text not null
- `description` text null

**Правила курса:**
- `student_payment_type` smallint not null  
  (1=PerLesson, 2=PerMonth, 3=PerCourse)
- `absence_policy` smallint not null  
  (1=Burn)
- `teacher_payment_type` smallint not null  
  (1=PerLesson, 2=PerHour)

**Статус:**
- `status` smallint not null (1=Active, 2=Archived)

**Soft delete:**
- `is_deleted` boolean not null default false
- `deleted_at` timestamptz null

**Audit:**
- `created_at`, `created_by`, `updated_at`, `updated_by`

**Ограничения:**
- name не обязательно уникален (часто бывают “Английский A1” несколько раз), но можно сделать уникальность + is_deleted=false при желании

**Индексы:**
- idx_courses_status (status)
- idx_courses_is_deleted (is_deleted)

---

## 2.4 groups

Группа — реализация курса во времени.

**Поля:**
- `id` uuid PK
- `name` text not null
- `course_id` uuid not null FK → courses(id)
- `teacher_id` uuid not null FK → users(id) (role=Teacher)
- `start_date` date not null
- `end_date` date null (плановая дата окончания)
- `capacity` int not null
- `status` smallint not null (1=Active, 2=Completed)

**Расписание группы (минимально):**
- `days_of_week` int not null  
  (битовая маска или другое представление, например: 1=Mon,2=Tue,...)
- `time_start` time not null
- `time_end` time not null

**Soft delete:**
- `is_deleted` boolean not null default false
- `deleted_at` timestamptz null

**Audit:**
- `created_at`, `created_by`, `updated_at`, `updated_by`

**Ограничения:**
- capacity > 0 (check)
- time_end > time_start (check)

**Индексы:**
- idx_groups_course (course_id)
- idx_groups_teacher (teacher_id)
- idx_groups_status (status)
- idx_groups_is_deleted (is_deleted)

---

## 2.5 group_students (связь группа ↔ студенты)

**Поля:**
- `group_id` uuid FK → groups(id)
- `student_id` uuid FK → users(id) (role=Student)
- `joined_at` timestamptz not null

**PK:**
- составной PK (`group_id`, `student_id`)

**Индексы:**
- idx_group_students_student (student_id)

**Правило:**
- добавление/удаление — это управление участием, не удаление студента.

---

## 2.6 lessons

Урок — базовая единица. История не удаляется.

**Поля:**
- `id` uuid PK
- `group_id` uuid not null FK → groups(id)
- `date` date not null
- `time_start` time not null
- `time_end` time not null
- `teacher_id` uuid not null FK → users(id) (role=Teacher)
- `status` smallint not null  
  (1=Planned, 2=Completed, 3=Cancelled)

**Audit:**
- `created_at`, `created_by`, `updated_at`, `updated_by`

**Ограничения:**
- time_end > time_start (check)

**Индексы:**
- idx_lessons_group_date (group_id, date)
- idx_lessons_teacher_date (teacher_id, date)
- idx_lessons_date (date)
- idx_lessons_status (status)

> Замена преподавателя = изменение teacher_id конкретного lesson (и фиксируется аудитом).

---

## 2.7 attendance_records

Посещаемость — запись на пару (урок, студент).

**Поля:**
- `lesson_id` uuid not null FK → lessons(id)
- `student_id` uuid not null FK → users(id) (role=Student)
- `status` smallint not null (1=Present, 2=Absent)
- `marked_at` timestamptz not null
- `marked_by` uuid not null FK → users(id) (role=Teacher или Admin)

**PK:**
- составной PK (`lesson_id`, `student_id`)

**Индексы:**
- idx_attendance_student (student_id)
- idx_attendance_lesson (lesson_id)

**Правила:**
- нельзя ставить посещаемость для cancelled урока (проверяется в Application)
- посещаемость не влияет на финансы

---

## 2.8 payments

Платёж — факт оплаты. Нет долгов, нет “ожидает”.

**Поля:**
- `id` uuid PK
- `payer_type` smallint not null (1=Student, 2=Parent)
- `payer_id` uuid not null FK → users(id)
- `target_type` smallint not null (1=Course, 2=Month, 3=Package)
- `target_ref` text not null  
  (например: "course:<courseId>", "month:2026-01", "package:EnglishA1-15")
- `amount` numeric(12,2) not null
- `paid_at` timestamptz not null
- `comment` text null

**Audit:**
- `created_at`, `created_by` (кто занёс платёж)

**Индексы:**
- idx_payments_paid_at (paid_at)
- idx_payments_payer (payer_id, paid_at)
- idx_payments_target (target_type)

**Ограничения:**
- amount > 0 (check)

---

## 3) Важные бизнес-ограничения (проверяются в Application)

1. Нельзя добавить студента в группу, если превышен `capacity`.
2. Нельзя добавить студента в группу, если курс запрещает параллельное обучение (если включим правило).
3. Нельзя отметить посещаемость на `Cancelled` урок.
4. Нельзя отменить урок со статусом `Completed` (или можно, но только админом — решается отдельно).
5. Нельзя “удалить” урок/платёж/посещаемость — только статус/коррекция.

---

## 4) Индексы, которые реально нужны (MVP)

- lessons по дате: `idx_lessons_date`
- lessons по учителю+дате: `idx_lessons_teacher_date`
- lessons по группе+дате: `idx_lessons_group_date`
- attendance по student: `idx_attendance_student`
- payments по paid_at: `idx_payments_paid_at`
- users по phone (активные): `uq_users_phone_active`

---

## 5) Примечания по soft delete

- При soft delete пользователя он исчезает из списков, но:
  - его уроки, посещаемость, платежи сохраняются
- В UI можно добавить фильтр “показать удалённых” только для админа (не обязательно для MVP)

---

## 6) Статус документа

Версия: MVP v1.0
Любые изменения структуры таблиц требуют обновления этого файла.
