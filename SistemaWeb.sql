-- Crear la base de datos
CREATE DATABASE SistemaWeb;
GO

-- Usar la base de datos
USE SistemaWeb;
GO

-- Crear la tabla de Roles
CREATE TABLE Roles (
    RolID INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol NVARCHAR(100) NOT NULL
);
GO

-- Crear la tabla de Usuarios
CREATE TABLE Users (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(100) NOT NULL,
    Contraseña NVARCHAR(255) NOT NULL,
    CorreoElectrónico NVARCHAR(255) NOT NULL UNIQUE,
	ClaveTemp BIT NOT NULL,
	Vigencia DATETIME NOT NULL,
    RolID INT FOREIGN KEY REFERENCES Roles(RolID)
);
GO

-- Crear la tabla de Noticias
CREATE TABLE Noticias (
    NoticiaID INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(255) NOT NULL,
    Contenido NVARCHAR(MAX) NOT NULL,
    FechaPublicacion DATETIME NOT NULL,
    AdministradorID INT FOREIGN KEY REFERENCES Users(UsuarioID)
);
GO

-- Crear la tabla de Solicitudes
CREATE TABLE Solicitudes(
    SolicitudID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT FOREIGN KEY REFERENCES Users(UsuarioID),
    DetalleSolicitud NVARCHAR(MAX) NOT NULL,
    Estado NVARCHAR(50) CHECK (Estado IN ('Pendiente', 'Resuelta', 'Desestimada')),
    FechaSolicitud DATETIME NOT NULL
);
GO

-- Crear la tabla de Inventario
CREATE TABLE Inventario (
    ArticuloID INT IDENTITY(1,1) PRIMARY KEY,
	Responsable INT FOREIGN KEY REFERENCES Users(UsuarioID),
    NombreArticulo NVARCHAR(255) NOT NULL,
    Descripcion NVARCHAR(MAX),
	FechaDeCreacion DATETIME  NOT NULL,
    Cantidad INT NOT NULL
);
GO

-- Crear la tabla de Facturas
CREATE TABLE Facturas (
    FacturaID INT IDENTITY(1,1) PRIMARY KEY,
	RegistroUsuario INT FOREIGN KEY REFERENCES Users(UsuarioID),
    Proveedor NVARCHAR(255) NOT NULL,
    Monto DECIMAL(18, 2) NOT NULL,
    FechaEmision DATETIME NOT NULL,
    FechaVencimiento DATETIME NOT NULL,
    Estado NVARCHAR(50) CHECK (Estado IN ('Pendiente', 'Pagada'))
);
GO

-- Crear la tabla de Pagos
CREATE TABLE Pagos (
    PagoID INT IDENTITY(1,1) PRIMARY KEY,
    FacturaID INT FOREIGN KEY REFERENCES Facturas(FacturaID),
    FechaPago DATETIME NOT NULL,
    MontoPagado DECIMAL(18, 2) NOT NULL,
    MetodoPago NVARCHAR(100) NOT NULL
);
GO

-- Crear la tabla de Logs
CREATE TABLE Logs (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT FOREIGN KEY REFERENCES Users(UsuarioID),
    Accion NVARCHAR(MAX) NOT NULL,
    FechaHora DATETIME NOT NULL
);
GO

-- Crear la tabla de Juegos
CREATE TABLE Games (
    JuegoID INT IDENTITY(1,1) PRIMARY KEY,
	Creador INT FOREIGN KEY REFERENCES Users(UsuarioID),
    NombreJuego NVARCHAR(255) NOT NULL,
    Descripción NVARCHAR(MAX)
);
GO

-- Crear la tabla de VersionesJuegos
CREATE TABLE VersionesJuegos (
    VersionID INT IDENTITY(1,1) PRIMARY KEY,
    JuegoID INT FOREIGN KEY REFERENCES Games(JuegoID),
    NumeroVersion NVARCHAR(50) NOT NULL,
    FechaLanzamiento DATETIME NOT NULL,
    DescripcionVersion NVARCHAR(MAX),
    Estado NVARCHAR(50) CHECK (Estado IN ('Activa', 'Inactiva'))
);
GO

-- Crear la tabla de ConfiguracionesJuegos
CREATE TABLE ConfiguracionesJuegos (
    ConfiguracionID INT IDENTITY(1,1) PRIMARY KEY,
    VersionID INT FOREIGN KEY REFERENCES VersionesJuegos(VersionID),
    ClaveConfiguracion NVARCHAR(255) NOT NULL,
    ValorConfiguracion NVARCHAR(MAX) NOT NULL
);
GO



-- Eliminar tablas en orden inverso para evitar problemas de dependencia

-- Eliminar la tabla de ConfiguracionesJuegos
DROP TABLE IF EXISTS ConfiguracionesJuegos;
GO

-- Eliminar la tabla de VersionesJuegos
DROP TABLE IF EXISTS VersionesJuegos;
GO

-- Eliminar la tabla de Juegos
DROP TABLE IF EXISTS Games;
GO

-- Eliminar la tabla de Actividades
DROP TABLE IF EXISTS Actividades;
GO

-- Eliminar la tabla de Logs
DROP TABLE IF EXISTS Logs;
GO

-- Eliminar la tabla de Pagos
DROP TABLE IF EXISTS Pagos;
GO

-- Eliminar la tabla de Facturas
DROP TABLE IF EXISTS Facturas;
GO

-- Eliminar la tabla de Inventario
DROP TABLE IF EXISTS Inventario;
GO

-- Eliminar la tabla de Solicitudes
DROP TABLE IF EXISTS Solicitudes;
GO

-- Eliminar la tabla de Noticias
DROP TABLE IF EXISTS Noticias;
GO

-- Eliminar la tabla de Usuarios
DROP TABLE IF EXISTS Users;
GO

-- Eliminar la tabla de Roles
DROP TABLE IF EXISTS Roles;
GO


INSERT INTO Roles (NombreRol) VALUES 
('Admin'),
('Profesor'),
('Estudiante'),
('Usuario');

INSERT INTO Users (NombreUsuario, Contraseña, CorreoElectrónico,ClaveTemp ,Vigencia , RolID) VALUES 
('admin_user', '$2a$12$IA9DGlCjQ56jgpTk6FisV.bSY/HyO/KJgXJ8o9cMc0Hhe.9zrpWw.', 'admin@example.com',0, GETDATE(),  1);
--  Password123#
INSERT INTO Noticias (Titulo, Contenido, FechaPublicacion, AdministradorID) VALUES 
('Nuevo Lanzamiento de Producto', 'Estamos emocionados de anunciar el lanzamiento de nuestro nuevo producto que revolucionará la industria.', '2024-10-01', 1),
('Actualización de Seguridad', 'Hemos implementado una nueva actualización de seguridad que mejora la protección de los datos de nuestros usuarios.', '2024-10-05', 1),
('Evento Especial', 'Únete a nosotros en nuestro evento especial el próximo mes. Habrá sorpresas y mucho más.', '2024-10-10', 1),
('Consejos de Uso', 'Aquí hay algunos consejos sobre cómo sacar el máximo provecho de nuestros productos y servicios.', '2024-10-15', 1);


