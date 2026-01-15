# 05 — Architecture & Project Structure (MVP)

Документ фиксирует финальную архитектуру и структуру проекта CRM для учебного центра.

Архитектура: **гибрид Clean Architecture + упрощённый Vertical Slice**  
Цель: понятная реализация MVP без архитектурного перегруза.

---

## 1. Общий подход

Проект реализуется как **монолит** с чётким разделением ответственности по проектам:

- Domain — бизнес-сущности и правила
- Application — сценарии (use-cases) и бизнес-логика
- Infrastructure — база данных, EF Core, внешние реализации
- WebApi — HTTP API, контракты, авторизация

Это **не микросервисы**, не CQRS, не MediatR.

---

## 2. Структура solution

```

Eureka.sln
/src
WebApi/
Application/
Domain/
Infrastructure/
/docs
01-product.md
02-domain.md
03-flows.md
04-setup.md
05-architecture.md
06-decisions.md
07-data-model.md
08-acceptance-tests.md
README.md

```

---

## 3. Правила зависимостей (строго)

Разрешённые зависимости:

- WebApi → Application
- Application → Domain
- Infrastructure → Application + Domain
- WebApi → Infrastructure (только для DI)

Запрещено:

- Domain → *что угодно*
- Application → Infrastructure
- WebApi → DbContext напрямую

---

## 4. Domain (чистая бизнес-модель)

Domain содержит **только сущности, enum и базовые классы**.  
Никакого EF Core, DTO, HTTP, логирования.

```

src/Domain/
Common/
EntityBase.cs
AuditableEntity.cs          // CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
SoftDeletableEntity.cs      // IsDeleted, DeletedAt
DomainException.cs

Users/
Role.cs                     // Admin, Teacher, Student, Parent
User.cs

Courses/
Course.cs
CoursePaymentType.cs        // PerLesson, PerMonth, PerCourse
TeacherPaymentType.cs       // PerLesson, PerHour
AbsencePolicy.cs            // Burn

Groups/
Group.cs
GroupSchedule.cs            // Value Object (days + time)
GroupStudent.cs             // N-N связь

Lessons/
Lesson.cs
LessonStatus.cs             // Planned, Completed, Cancelled

Attendance/
AttendanceRecord.cs
AttendanceStatus.cs         // Present, Absent

Payments/
Payment.cs
PaymentTargetType.cs        // Course, Month, Package

```

### Soft Delete (MVP)

Soft delete реализуется **двумя полями**:
- `IsDeleted`
- `DeletedAt`

Используется для:
- User
- Course
- Group

НЕ используется для:
- Lesson
- AttendanceRecord
- Payment

Исторические данные не удаляются.

---

## 5. Application (бизнес-логика и сценарии)

Application содержит **все use-cases системы**.  
Структура — по модулям (понятно), внутри — по сценариям.

```

src/Application/
Abstractions/
IAppDbContext.cs
ICurrentUser.cs
IClock.cs

Common/
Result.cs
Errors.cs
Paging.cs

Modules/

Users/
  Dtos/
  UseCases/
    CreateUser/
    Login/

Courses/
  Dtos/
  UseCases/
    CreateCourse/
    UpdateCourse/
    ListCourses/
    ArchiveCourse/

Groups/
  Dtos/
  UseCases/
    CreateGroup/
    UpdateGroup/
    AddStudentToGroup/
    RemoveStudentFromGroup/
    CloseGroup/

Lessons/
  Dtos/
  UseCases/
    GenerateLessonsForGroup/
    RescheduleLesson/
    CancelLesson/
    SubstituteTeacher/
    ListLessonsByDate/

Attendance/
  Dtos/
  UseCases/
    MarkAttendance/
    GetLessonAttendance/

Payments/
  Dtos/
  UseCases/
    CreatePayment/
    ListPayments/
    PaymentsReport/

```

### Валидация

- DataAnnotations — допустимы только для базовых полей
- Бизнес-валидация (правила) — **только в Application**

---

## 6. Infrastructure (EF Core + PostgreSQL)

Infrastructure — реализация доступа к данным и внешним сервисам.

```

src/Infrastructure/
Persistence/
AppDbContext.cs
Configurations/
UserConfig.cs
CourseConfig.cs
GroupConfig.cs
GroupStudentConfig.cs
LessonConfig.cs
AttendanceRecordConfig.cs
PaymentConfig.cs
Migrations/

Services/
CurrentUserService.cs
Clock.cs

DependencyInjection.cs

```

### EF Core

- PostgreSQL 16
- EF Core 8
- Миграции обязательны
- Global Query Filter для `IsDeleted = false`

---

## 7. WebApi (HTTP слой)

WebApi отвечает за:
- приём HTTP-запросов
- валидацию DTO
- вызов Application
- возврат ответа

```

src/WebApi/
Controllers/
v1/
UsersController.cs
CoursesController.cs
GroupsController.cs
LessonsController.cs
AttendanceController.cs
PaymentsController.cs

Contracts/
Users/
Courses/
Groups/
Lessons/
Attendance/
Payments/

Auth/
JwtOptions.cs
JwtTokenService.cs

Middleware/
ExceptionHandlingMiddleware.cs
RequestLoggingMiddleware.cs

DependencyInjection.cs
Program.cs
appsettings.json

```

### DTO

- DTO живут **только в WebApi.Contracts**
- Domain-сущности никогда не возвращаются наружу

---

## 8. Архитектурные запреты (MVP)

В MVP **запрещено**:
- Repository pattern
- CQRS / MediatR
- микросервисы
- гибкие роли
- сложная аналитика
- автоматические финансы
- удаление исторических данных

---

## 9. Статус документа

Актуально для:
- CRM MVP v1
- .NET 8
- ASP.NET Core 8
- EF Core 8
- PostgreSQL 16

Любые изменения архитектуры требуют обновления этого файла.