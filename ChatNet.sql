create database ChatNet
go

use ChatNet
go

create table Usuarios(
IdUsuario int not null primary key identity(1,1),
Nombre varchar(max) null,
Apellido varchar(max) null,
Correo varchar(max) null,
Foto varchar(max) null,
Password varchar(max) not null
);
go

create table Mensajes(
IdMensaje int not null primary key identity(1,1),
IdEnvia int not null foreign key references Usuarios(IdUsuario) ,
IdResive int not null foreign key references Usuarios(IdUsuario),
Mensaje varchar(max) null
);
go

create or alter proc sp_Usuario_GetUsuario 
@idusuario int 
as
begin
	select * from Usuarios where IdUsuario = @idusuario
end
go

create or alter proc sp_Usuario_Lst 
@idusuario int 
as
begin
	select * from Usuarios where IdUsuario not in(@idusuario)
end
go

create or alter proc sp_Mensaje_GetUsuario
@idusuario int,@idamigo int
as
begin
	select m.* from Mensajes as m inner join Usuarios as u 
	on u.IdUsuario = m.IdEnvia
	where (IdEnvia = @idusuario and IdResive = @idamigo) or (IdEnvia = @idamigo and IdResive = @idusuario)
	order by m.IdMensaje asc;
end
go

create or alter proc sp_EnviarMensaje 
@idenvia int ,@idrecibe int ,@mensaje varchar(max)
as
begin
	insert into Mensajes(IdEnvia,IdResive,Mensaje)values(@idenvia,@idrecibe,@mensaje);
end
go

select * from Mensajes