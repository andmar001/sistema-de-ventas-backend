# CapaDAL
Acceso a la base de datos

IGenericRepository es una interfaz generic para todas las clases
- IGenericRepository<TModel> where TModel : class //forma de trabajar de manera dinamica con todos los modelos

# Capa DTO 

- SesionDTO  -- guarda la sesion del usuario que se ha logueado

# Utility
Automapper
Mapeo de clases 

clase origen - clase destino
CreateMap<Rol, RolDTO>();

- que los campos tengan el mismo nombre de los campo para la relacion

-- personalizacion de datos, cuando las clases DTO tiene diferentes tipos de datos
# ForMember



# Proceso

- creacion de las capas
1 - capa DAL - contexto con la base de datos



Instalaciones -
Automapper
Automapper extension


# db - Por medio de Scaffold