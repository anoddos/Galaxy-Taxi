CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Accounts" (
    "Id" uuid NOT NULL,
    "AccountType" integer NOT NULL,
    "Email" text NOT NULL,
    "CompanyName" text NOT NULL,
    "PasswordHash" text NOT NULL,
    CONSTRAINT "PK_Accounts" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220730194744_InitialCreate', '6.0.7');

COMMIT;

