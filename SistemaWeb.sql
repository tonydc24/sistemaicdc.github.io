-- Crear la base de datos
CREATE DATABASE SistemaWeb;
GO

-- Usar la base de datos
USE [bdsistemaweb];
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
	FechaEvento DATETIME NOT NULL,
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

CREATE TABLE PreguntasTrivia (
    Id INT IDENTITY(1,1) PRIMARY KEY ,
    Nivel VARCHAR(20) NOT NULL, -- 'Basico', 'Medio', 'Dificil'
    Pregunta TEXT NOT NULL,
    RespuestaCorrecta VARCHAR(255) NOT NULL,
    RespuestaIncorrecta1 VARCHAR(255) NOT NULL,
    RespuestaIncorrecta2 VARCHAR(255) NOT NULL,
    RespuestaIncorrecta3 VARCHAR(255) NOT NULL
);


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
INSERT INTO Noticias (Titulo, Contenido, FechaPublicacion,FechaEvento, AdministradorID) VALUES 
('Nuevo Lanzamiento de Producto', 'Estamos emocionados de anunciar el lanzamiento de nuestro nuevo producto que revolucionará la industria.', '2024-10-01',GETDATE(), 1),
('Actualización de Seguridad', 'Hemos implementado una nueva actualización de seguridad que mejora la protección de los datos de nuestros usuarios.', '2024-10-05',GETDATE(), 1),
('Evento Especial', 'Únete a nosotros en nuestro evento especial el próximo mes. Habrá sorpresas y mucho más.', '2024-10-10',GETDATE(), 1),
('Consejos de Uso', 'Aquí hay algunos consejos sobre cómo sacar el máximo provecho de nuestros productos y servicios.', '2024-10-15',GETDATE(), 1);


INSERT INTO PreguntasTrivia (Nivel, Pregunta, RespuestaCorrecta, RespuestaIncorrecta1, RespuestaIncorrecta2, RespuestaIncorrecta3)
VALUES
('Basico', '¿Quién creó el cielo y la tierra?', 'Dios', 'Jesús', 'Noé', 'Moisés'),
('Basico', '¿Cómo se llamaron los primeros hombres en la Biblia?', 'Adán y Eva', 'Caín y Abel', 'José y María', 'Moisés y Aarón'),
('Basico', '¿Qué hizo Dios al séptimo día después de crear el mundo?', 'Descansó', 'Trabajó', 'Comió', 'Durmió'),
('Basico', '¿Quién salvó a los animales en un arca?', 'Noé', 'Moisés', 'Abraham', 'Pedro'),
('Basico', '¿Cómo se llamaba la mamá de Jesús?', 'María', 'Eva', 'Sara', 'Marta');


INSERT INTO PreguntasTrivia (Nivel, Pregunta, RespuestaCorrecta, RespuestaIncorrecta1, RespuestaIncorrecta2, RespuestaIncorrecta3)
VALUES
('Medio', '¿Cuántos días y noches llovió durante el diluvio?', '40', '30', '20', '50'),
('Medio', '¿Cómo se llamaba el padre de Jesús en la tierra?', 'José', 'Abraham', 'Isaac', 'Pedro'),
('Medio', '¿Qué comida multiplicó Jesús para alimentar a mucha gente?', 'Panes y peces', 'Maná y codornices', 'Panes y vino', 'Frutas y peces'),
('Medio', '¿Cuántos discípulos tenía Jesús?', '12', '10', '11', '13'),
('Medio', '¿Cómo llamó Jesús a sus seguidores?', 'Discípulos', 'Apóstoles', 'Feligreses', 'Amigos');


INSERT INTO PreguntasTrivia (Nivel, Pregunta, RespuestaCorrecta, RespuestaIncorrecta1, RespuestaIncorrecta2, RespuestaIncorrecta3)
VALUES
('Dificil', '¿Cuántos días tardó Dios en crear el mundo?', '6 días y descansó el séptimo', '7 días', '5 días', '10 días'),
('Dificil', '¿Quién abrió el Mar Rojo para que los israelitas cruzaran?', 'Moisés', 'Abraham', 'Josué', 'David'),
('Dificil', '¿Qué señal dio Dios después del diluvio?', 'Un arco iris', 'Un trueno', 'Un fuego', 'Una nube'),
('Dificil', '¿Qué usó David para vencer a Goliat?', 'Una honda y una piedra', 'Una espada', 'Un arco y flecha', 'Sus manos'),
('Dificil', '¿Dónde nació Jesús?', 'Belén', 'Nazaret', 'Jerusalén', 'Egipto');