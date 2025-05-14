-- Create DB Tables
-- *Al no haber especificaciones de las características de las columnas, todas serán NOT NULL y las FOREIGN KEYS
-- 	se actualizarán ON UPDATE y rechazarán las operaciones DELETE de las filas referenciadas.
-- *Los valores DEFAULT se basan en la descripción del sistema y para reducir los valores necesarios para crear una fila.
-- *El tamaño y formato de las columnas se basó en la tabla Usuarios definida en el planteamiento del sistema y
-- 	en base a la descripción del mismo.
-- *La relación entre Usuarios y Roles es de 1 a Muchos, pero de ser necesario se puede agregar una tabla intermedia
--	que permita la relación de Muchos a Muchos.
-- Este script esta diseñado para utilizarse en SQL Server

CREATE TABLE Roles (
  idRol INT IDENTITY(1,1) PRIMARY KEY,
  nombre VARCHAR(25) NOT NULL,
);

CREATE TABLE Usuarios (
  idUsuario INT IDENTITY(1,1) PRIMARY KEY,
  nombre VARCHAR(100) NOT NULL,
  correo VARCHAR(50) NOT NULL,
  contrasena VARCHAR(25) NOT NULL,
  idRol INT NOT NULL,
  estatus INT NOT NULL DEFAULT 1, -- 1: Activo 0: Inactivo
  FOREIGN KEY (idRol) REFERENCES Roles(idRol) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE Productos (
  idProducto INT IDENTITY(1,1) PRIMARY KEY,
  nombre VARCHAR(100) NOT NULL,
  precio DECIMAL(16,2) NOT NULL,
  cantidad INT NOT NULL DEFAULT 0,
  estatus INT NOT NULL DEFAULT 1 -- 1: Activo 0: Inactivo
);

CREATE TABLE Historico (
  idHistorico INT IDENTITY(1,1) PRIMARY KEY,
  idProducto INT NOT NULL,
  tipo CHAR(1) NOT NULL,
  idUsuario INT NOT NULL,
  fecha DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (idProducto) REFERENCES Productos(idProducto) ON UPDATE CASCADE ON DELETE NO ACTION,
  FOREIGN KEY (idUsuario) REFERENCES Usuarios(idUsuario) ON UPDATE CASCADE ON DELETE NO ACTION
);