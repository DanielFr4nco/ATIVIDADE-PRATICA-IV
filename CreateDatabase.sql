
USE master;
GO
IF DB_ID('ServicosTecnicosDB') IS NULL
    CREATE DATABASE ServicosTecnicosDB;
GO
USE ServicosTecnicosDB;
GO
IF OBJECT_ID('Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE Clientes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(100) NOT NULL,
        Telefone NVARCHAR(20) NOT NULL,
        Email NVARCHAR(100) NOT NULL
    );
END;
GO
IF OBJECT_ID('Tecnicos', 'U') IS NULL
BEGIN
    CREATE TABLE Tecnicos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nome NVARCHAR(100) NOT NULL,
        Especialidade NVARCHAR(100) NOT NULL
    );
END;
GO
IF OBJECT_ID('OrdensServico', 'U') IS NULL
BEGIN
    CREATE TABLE OrdensServico (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ClienteId INT NOT NULL,
        TecnicoId INT NULL,
        TipoServico NVARCHAR(30) NOT NULL,
        Descricao NVARCHAR(500) NOT NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT 'Aberta',
        DataAbertura DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        DataExecucao DATETIME2 NULL,
        DataFinalizacao DATETIME2 NULL,
        Valor DECIMAL(10,2) NULL,
        CONSTRAINT FK_OrdensServico_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
        CONSTRAINT FK_OrdensServico_Tecnicos FOREIGN KEY (TecnicoId) REFERENCES Tecnicos(Id),
        CONSTRAINT CK_OrdensServico_Tipo CHECK (TipoServico IN ('Manutencao', 'Instalacao', 'Suporte')),
        CONSTRAINT CK_OrdensServico_Status CHECK (Status IN ('Aberta', 'EmExecucao', 'Finalizada'))
    );
END;
GO

-- Atualiza os valores e os CHECKs caso a tabela ja exista.
IF OBJECT_ID('OrdensServico', 'U') IS NOT NULL
BEGIN
    IF OBJECT_ID('CK_OrdensServico_Tipo', 'C') IS NOT NULL
        ALTER TABLE OrdensServico DROP CONSTRAINT CK_OrdensServico_Tipo;

    IF OBJECT_ID('CK_OrdensServico_Status', 'C') IS NOT NULL
        ALTER TABLE OrdensServico DROP CONSTRAINT CK_OrdensServico_Status;

    UPDATE OrdensServico
    SET TipoServico = CASE
        WHEN TipoServico = N'Manutenção' THEN 'Manutencao'
        WHEN TipoServico = N'Instalação' THEN 'Instalacao'
        ELSE TipoServico
    END;

    ALTER TABLE OrdensServico WITH CHECK ADD
        CONSTRAINT CK_OrdensServico_Tipo
        CHECK (TipoServico IN ('Manutencao', 'Instalacao', 'Suporte'));

    ALTER TABLE OrdensServico WITH CHECK ADD
        CONSTRAINT CK_OrdensServico_Status
        CHECK (Status IN ('Aberta', 'EmExecucao', 'Finalizada'));
END;
GO
