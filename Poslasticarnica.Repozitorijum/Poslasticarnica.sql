IF DB_ID('PoslasticarnicaDB') IS NULL
BEGIN
    CREATE DATABASE PoslasticarnicaDB;
END
GO

USE PoslasticarnicaDB;
GO

IF OBJECT_ID('TipTorte', 'U') IS NULL
BEGIN
    CREATE TABLE TipTorte
    (
        TipTorteID INT IDENTITY(1,1) PRIMARY KEY,
        Naziv NVARCHAR(100) NOT NULL
    );
END
GO

IF OBJECT_ID('Proizvod', 'U') IS NULL
BEGIN
    CREATE TABLE Proizvod
    (
        ProizvodID INT IDENTITY(1,1) PRIMARY KEY,
        Naziv NVARCHAR(100) NOT NULL,
        Kategorija NVARCHAR(50) NOT NULL,
        Cena DECIMAL(18,2) NOT NULL
    );
END
GO

IF OBJECT_ID('Korisnik', 'U') IS NULL
BEGIN
    CREATE TABLE Korisnik
    (
        KorisnikID INT IDENTITY(1,1) PRIMARY KEY,
        KorisnickoIme NVARCHAR(100) NOT NULL UNIQUE,
        Lozinka NVARCHAR(100) NOT NULL,
        Uloga NVARCHAR(50) NOT NULL
    );
END
GO

IF OBJECT_ID('Narudzbina', 'U') IS NULL
BEGIN
    CREATE TABLE Narudzbina
    (
        NarudzbinaID INT IDENTITY(1,1) PRIMARY KEY,
        ImePrezimeKupca NVARCHAR(100) NOT NULL,
        BrojTelefona NVARCHAR(20) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        DatumKreiranja DATETIME NOT NULL,
        DatumPreuzimanja DATETIME NOT NULL,
        Status NVARCHAR(50) NOT NULL,
        UkupnaCena DECIMAL(18,2) NOT NULL
            CONSTRAINT DF_Narudzbina_UkupnaCena DEFAULT 0,
        Napomena NVARCHAR(500) NULL,
        TipTorteID INT NOT NULL,
        CONSTRAINT FK_Narudzbina_TipTorte
            FOREIGN KEY (TipTorteID)
            REFERENCES TipTorte(TipTorteID)
    );
END
GO

IF OBJECT_ID('StavkaNarudzbine', 'U') IS NULL
BEGIN
    CREATE TABLE StavkaNarudzbine
    (
        StavkaNarudzbineID INT IDENTITY(1,1) PRIMARY KEY,
        NarudzbinaID INT NOT NULL,
        ProizvodID INT NOT NULL,
        Kolicina INT NOT NULL,
        CONSTRAINT FK_StavkaNarudzbine_Narudzbina
            FOREIGN KEY (NarudzbinaID)
            REFERENCES Narudzbina(NarudzbinaID),
        CONSTRAINT FK_StavkaNarudzbine_Proizvod
            FOREIGN KEY (ProizvodID)
            REFERENCES Proizvod(ProizvodID),
        CONSTRAINT CK_StavkaNarudzbine_Kolicina
            CHECK (Kolicina > 0)
    );
END
GO

CREATE OR ALTER PROCEDURE VratiSveNarudzbine
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        NarudzbinaID,
        ImePrezimeKupca,
        BrojTelefona,
        Email,
        DatumKreiranja,
        DatumPreuzimanja,
        Status,
        UkupnaCena,
        Napomena,
        TipTorteID
    FROM Narudzbina;
END
GO

CREATE OR ALTER PROCEDURE VratiNarudzbinuPoID
    @NarudzbinaID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        NarudzbinaID,
        ImePrezimeKupca,
        BrojTelefona,
        Email,
        DatumKreiranja,
        DatumPreuzimanja,
        Status,
        UkupnaCena,
        Napomena,
        TipTorteID
    FROM Narudzbina
    WHERE NarudzbinaID = @NarudzbinaID;
END
GO

CREATE OR ALTER PROCEDURE DodajNarudzbinu
    @ImePrezimeKupca NVARCHAR(100),
    @BrojTelefona NVARCHAR(20),
    @Email NVARCHAR(100),
    @DatumKreiranja DATETIME,
    @DatumPreuzimanja DATETIME,
    @Status NVARCHAR(50),
    @UkupnaCena DECIMAL(18,2),
    @Napomena NVARCHAR(500),
    @TipTorteID INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Narudzbina
    (
        ImePrezimeKupca,
        BrojTelefona,
        Email,
        DatumKreiranja,
        DatumPreuzimanja,
        Status,
        UkupnaCena,
        Napomena,
        TipTorteID
    )
    VALUES
    (
        @ImePrezimeKupca,
        @BrojTelefona,
        @Email,
        @DatumKreiranja,
        @DatumPreuzimanja,
        @Status,
        @UkupnaCena,
        @Napomena,
        @TipTorteID
    );
END
GO

CREATE OR ALTER PROCEDURE IzmeniNarudzbinu
    @NarudzbinaID INT,
    @ImePrezimeKupca NVARCHAR(100),
    @BrojTelefona NVARCHAR(20),
    @Email NVARCHAR(100),
    @DatumKreiranja DATETIME,
    @DatumPreuzimanja DATETIME,
    @Status NVARCHAR(50),
    @UkupnaCena DECIMAL(18,2),
    @Napomena NVARCHAR(500),
    @TipTorteID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Narudzbina
    SET
        ImePrezimeKupca = @ImePrezimeKupca,
        BrojTelefona = @BrojTelefona,
        Email = @Email,
        DatumKreiranja = @DatumKreiranja,
        DatumPreuzimanja = @DatumPreuzimanja,
        Status = @Status,
        UkupnaCena = @UkupnaCena,
        Napomena = @Napomena,
        TipTorteID = @TipTorteID
    WHERE NarudzbinaID = @NarudzbinaID;
END
GO

CREATE OR ALTER PROCEDURE ObrisiNarudzbinu
    @NarudzbinaID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Narudzbina
    WHERE NarudzbinaID = @NarudzbinaID;
END
GO