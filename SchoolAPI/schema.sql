IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [RegistrationStatuses] (
    [Id] varchar(50) NOT NULL,
    [Name] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_RegistrationStatuses] PRIMARY KEY ([Id])
);

CREATE TABLE [RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(max) NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id])
);

CREATE TABLE [Roles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NULL,
    [NormalizedName] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

CREATE TABLE [SchoolLevels] (
    [Id] varchar(50) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_SchoolLevels] PRIMARY KEY ([Id]),
    CONSTRAINT [CHK_SCHOOLLEVEL_NAME_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Name]))) > 0)
);

CREATE TABLE [Subjects] (
    [Id] varchar(50) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Code] varchar(50) NOT NULL,
    CONSTRAINT [PK_Subjects] PRIMARY KEY ([Id]),
    CONSTRAINT [CHK_SUBJECT_CODE_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Code]))) > 0),
    CONSTRAINT [CHK_SUBJECT_NAME_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Name]))) > 0)
);

CREATE TABLE [UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(max) NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id])
);

CREATE TABLE [UserLogins] (
    [LoginProvider] nvarchar(max) NOT NULL,
    [ProviderKey] nvarchar(max) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(max) NULL
);

CREATE TABLE [UserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId])
);

CREATE TABLE [Users] (
    [Id] nvarchar(450) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [UserName] nvarchar(max) NULL,
    [NormalizedUserName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [UserTokens] (
    [UserId] nvarchar(max) NULL,
    [LoginProvider] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Value] nvarchar(max) NULL
);

CREATE TABLE [Levels] (
    [Id] varchar(50) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [SchoolLevelId] varchar(50) NOT NULL,
    CONSTRAINT [PK_Levels] PRIMARY KEY ([Id]),
    CONSTRAINT [CHK_LEVEL_NAME_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Name]))) > 0),
    CONSTRAINT [FK_Levels_SchoolLevels_SchoolLevelId] FOREIGN KEY ([SchoolLevelId]) REFERENCES [SchoolLevels] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Teachers] (
    [Id] varchar(50) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Specialization] nvarchar(50) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Teachers] PRIMARY KEY ([Id]),
    CONSTRAINT [CHK_TEACHER_NAME_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Name]))) > 0),
    CONSTRAINT [FK_Teachers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Classes] (
    [Id] varchar(50) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Capacity] int NOT NULL DEFAULT 50,
    [LevelId] varchar(50) NOT NULL,
    CONSTRAINT [PK_Classes] PRIMARY KEY ([Id]),
    CONSTRAINT [CHK_CAPACITY_NOT_EMPTY] CHECK ([Capacity] > 0),
    CONSTRAINT [CHK_CLASS_NAME_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Name]))) > 0),
    CONSTRAINT [FK_Classes_Levels_LevelId] FOREIGN KEY ([LevelId]) REFERENCES [Levels] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [ClassSubjects] (
    [Id] varchar(50) NOT NULL,
    [ClassId] varchar(50) NOT NULL,
    [SubjectId] varchar(50) NOT NULL,
    CONSTRAINT [PK_ClassSubjects] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClassSubjects_Classes_ClassId] FOREIGN KEY ([ClassId]) REFERENCES [Classes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ClassSubjects_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Schedules] (
    [Id] varchar(50) NOT NULL,
    [Day] nvarchar(20) NOT NULL,
    [StartTime] time NOT NULL,
    [EndTime] time NOT NULL,
    [ClassId] varchar(50) NOT NULL,
    [SubjectId] varchar(50) NOT NULL,
    [TeacherId] varchar(50) NOT NULL,
    CONSTRAINT [PK_Schedules] PRIMARY KEY ([Id]),
    CONSTRAINT [CHK_DAYOFWEEK_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Day]))) > 0),
    CONSTRAINT [CHK_STARTTIME_BEFORE_ENDTIME] CHECK ([StartTime] < [EndTime]),
    CONSTRAINT [FK_Schedules_Classes_ClassId] FOREIGN KEY ([ClassId]) REFERENCES [Classes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Schedules_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Schedules_Teachers_TeacherId] FOREIGN KEY ([TeacherId]) REFERENCES [Teachers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Students] (
    [Id] varchar(50) NOT NULL,
    [Code] varchar(50) NOT NULL,
    [FullName] nvarchar(100) NOT NULL,
    [LatinName] nvarchar(100) NOT NULL,
    [Gender] nvarchar(10) NOT NULL,
    [DateOfBirth] date NOT NULL,
    [PlaceOfBirth] varchar(100) NOT NULL,
    [BackgroundStudy] nvarchar(100) NOT NULL,
    [FatherName] nvarchar(100) NOT NULL,
    [MotherName] nvarchar(100) NOT NULL,
    [Contact] varchar(100) NOT NULL,
    [Address] nvarchar(100) NOT NULL,
    [LevelId] varchar(50) NOT NULL,
    [ClassId] varchar(50) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id]),
    CONSTRAINT [CHK_ADDRESS_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Address]))) > 0),
    CONSTRAINT [CHK_BACKGROUNDSTUDY_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([BackgroundStudy]))) > 0),
    CONSTRAINT [CHK_CONTACT_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Contact]))) > 0),
    CONSTRAINT [CHK_FATHERNAME_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([FatherName]))) > 0),
    CONSTRAINT [CHK_FULLNAME_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([FullName]))) > 0),
    CONSTRAINT [CHK_LATINNAME_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([LatinName]))) > 0),
    CONSTRAINT [CHK_MOTHERNAME_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([MotherName]))) > 0),
    CONSTRAINT [CHK_POB_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([PlaceOfBirth]))) > 0),
    CONSTRAINT [FK_Students_Classes_ClassId] FOREIGN KEY ([ClassId]) REFERENCES [Classes] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Levels_LevelId] FOREIGN KEY ([LevelId]) REFERENCES [Levels] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Students_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [TeacherSubjectClasses] (
    [Id] varchar(50) NOT NULL,
    [TeacherId] varchar(50) NOT NULL,
    [SubjectId] varchar(50) NOT NULL,
    [ClassId] varchar(50) NOT NULL,
    CONSTRAINT [PK_TeacherSubjectClasses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TeacherSubjectClasses_Classes_ClassId] FOREIGN KEY ([ClassId]) REFERENCES [Classes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TeacherSubjectClasses_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TeacherSubjectClasses_Teachers_TeacherId] FOREIGN KEY ([TeacherId]) REFERENCES [Teachers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Payments] (
    [Id] varchar(50) NOT NULL,
    [Type] nvarchar(20) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Method] varchar(20) NOT NULL,
    [ReferenceNumber] varchar(50) NULL,
    [SlipURL] varchar(255) NULL,
    [Status] nvarchar(20) NOT NULL DEFAULT N'Pending',
    [PaidAt] datetime2 NULL,
    [StudentId] varchar(50) NOT NULL,
    [ReceivedBy] nvarchar(450) NOT NULL,
    [VerifiedBy] nvarchar(450) NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
    CONSTRAINT [CHK_AMOUNT_POSITIVE] CHECK ([Amount] > 0),
    CONSTRAINT [CHK_METHOD_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Method]))) > 0),
    CONSTRAINT [CHK_TYPE_NOT_EMPTY] CHECK (LEN(LTRIM(RTRIM([Type]))) > 0),
    CONSTRAINT [FK_Payments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Payments_Users_ReceivedBy] FOREIGN KEY ([ReceivedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Payments_Users_VerifiedBy] FOREIGN KEY ([VerifiedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Registrations] (
    [Id] varchar(50) NOT NULL,
    [StudentId] varchar(50) NOT NULL,
    [ClassId] varchar(50) NOT NULL,
    [StatusId] varchar(50) NOT NULL,
    [ApprovedBy] nvarchar(450) NULL,
    [ApprovedAt] datetime2 NULL,
    [RejectedBy] nvarchar(450) NULL,
    [RejectedAt] datetime2 NULL,
    [RejectionReason] varchar(255) NULL,
    [Notes] nvarchar(100) NULL,
    [CreatedAt] date NOT NULL,
    CONSTRAINT [PK_Registrations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Registrations_Classes_ClassId] FOREIGN KEY ([ClassId]) REFERENCES [Classes] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Registrations_RegistrationStatuses_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [RegistrationStatuses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Registrations_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Registrations_Users_ApprovedBy] FOREIGN KEY ([ApprovedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Registrations_Users_RejectedBy] FOREIGN KEY ([RejectedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Waitlists] (
    [Id] varchar(50) NOT NULL,
    [StudentId] varchar(50) NOT NULL,
    [ClassId] varchar(50) NOT NULL,
    [Notes] nvarchar(255) NULL,
    [RequestedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Waitlists] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Waitlists_Classes_ClassId] FOREIGN KEY ([ClassId]) REFERENCES [Classes] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Waitlists_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Classes_LevelId] ON [Classes] ([LevelId]);

CREATE UNIQUE INDEX [IX_Classes_Name] ON [Classes] ([Name]) WHERE [Name] <> '';

CREATE INDEX [IX_ClassSubjects_ClassId] ON [ClassSubjects] ([ClassId]);

CREATE INDEX [IX_ClassSubjects_SubjectId] ON [ClassSubjects] ([SubjectId]);

CREATE UNIQUE INDEX [IX_Levels_Name] ON [Levels] ([Name]) WHERE [Name] <> '';

CREATE INDEX [IX_Levels_SchoolLevelId] ON [Levels] ([SchoolLevelId]);

CREATE INDEX [IX_Payments_ReceivedBy] ON [Payments] ([ReceivedBy]);

CREATE INDEX [IX_Payments_StudentId] ON [Payments] ([StudentId]);

CREATE INDEX [IX_Payments_VerifiedBy] ON [Payments] ([VerifiedBy]);

CREATE INDEX [IX_Registrations_ApprovedBy] ON [Registrations] ([ApprovedBy]);

CREATE INDEX [IX_Registrations_ClassId] ON [Registrations] ([ClassId]);

CREATE INDEX [IX_Registrations_RejectedBy] ON [Registrations] ([RejectedBy]);

CREATE INDEX [IX_Registrations_StatusId] ON [Registrations] ([StatusId]);

CREATE INDEX [IX_Registrations_StudentId] ON [Registrations] ([StudentId]);

CREATE UNIQUE INDEX [IX_RegistrationStatuses_Name] ON [RegistrationStatuses] ([Name]);

CREATE INDEX [IX_Schedules_ClassId] ON [Schedules] ([ClassId]);

CREATE INDEX [IX_Schedules_SubjectId] ON [Schedules] ([SubjectId]);

CREATE INDEX [IX_Schedules_TeacherId] ON [Schedules] ([TeacherId]);

CREATE UNIQUE INDEX [IX_SchoolLevels_Name] ON [SchoolLevels] ([Name]) WHERE [Name] <> '';

CREATE INDEX [IX_Students_ClassId] ON [Students] ([ClassId]);

CREATE UNIQUE INDEX [IX_Students_Code] ON [Students] ([Code]) WHERE code <> '';

CREATE INDEX [IX_Students_LevelId] ON [Students] ([LevelId]);

CREATE UNIQUE INDEX [IX_Students_UserId] ON [Students] ([UserId]);

CREATE UNIQUE INDEX [IX_Subjects_Name] ON [Subjects] ([Name]) WHERE [Name] <> '';

CREATE INDEX [IX_Teachers_Name] ON [Teachers] ([Name]) WHERE [Name] <> '';

CREATE UNIQUE INDEX [IX_Teachers_UserId] ON [Teachers] ([UserId]);

CREATE INDEX [IX_TeacherSubjectClasses_ClassId] ON [TeacherSubjectClasses] ([ClassId]);

CREATE INDEX [IX_TeacherSubjectClasses_SubjectId] ON [TeacherSubjectClasses] ([SubjectId]);

CREATE INDEX [IX_TeacherSubjectClasses_TeacherId] ON [TeacherSubjectClasses] ([TeacherId]);

CREATE INDEX [IX_Waitlists_ClassId] ON [Waitlists] ([ClassId]);

CREATE INDEX [IX_Waitlists_StudentId] ON [Waitlists] ([StudentId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260131171944_InitialCreate', N'10.0.0');

COMMIT;
GO

