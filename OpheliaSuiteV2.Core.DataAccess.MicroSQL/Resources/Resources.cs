﻿// <auto-generated />

namespace OpheliaSuiteV2.Core.DataAccess.MicroSQL
{
	using System;
    using System.CodeDom.Compiler;
	using OpheliaSuiteV2.Core.DataAccess.MicroSQL.Exceptions;

	// <summary>
	// Cadenas de recursos fuertemente tipadas.
	// </summary>
	[GeneratedCode("Resources.tt", "1.0.0.0")]
	internal static class Strings
	{
		// <summary>
		// Retorna una cadena como: "El parámetro ({0}) no tiene un valor válido"
		// </summary>
		internal static string ArgumentException(object p0)
		{
			return string.Format("El parámetro ({0}) no tiene un valor válido", p0);
		}

		// <summary>
		// Retorna una cadena como: "Está intentando agregar una entidad ({0}) que ya existe en el contexto de datos"
		// </summary>
		internal static string EntityAlreadyExistsException(object p0)
		{
			return string.Format("Está intentando agregar una entidad ({0}) que ya existe en el contexto de datos", p0);
		}

		// <summary>
		// Retorna una cadena como: "La propiedad foránea ({0}) no existe en la entidad ({1})"
		// </summary>
		internal static string ForeignPropertyDontExistsException(object p0, object p1)
		{
			return string.Format("La propiedad foránea ({0}) no existe en la entidad ({1})", p0, p1);
		}

		// <summary>
		// Retorna una cadena como: "El tipo de contexto ({0}) no es válido"
		// </summary>
		internal static string InvalidDbContextTypeException(object p0)
		{
			return string.Format("El tipo de contexto ({0}) no es válido", p0);
		}

		// <summary>
		// Retorna una cadena como: "La entidad ({0}) tiene una cantidad inválida de propiedades marcadas como llave principal"
		// </summary>
		internal static string InvalidEntityKeyException(object p0)
		{
			return string.Format("La entidad ({0}) tiene una cantidad inválida de propiedades marcadas como llave principal", p0);
		}

		// <summary>
		// Retorna una cadena como: "El tipo de la propiedad de foránea ({0}) no es corresponde con el de la propiedad local"
		// </summary>
		internal static string InvalidForeignPropertyException(object p0)
		{
			return string.Format("El tipo de la propiedad de foránea ({0}) no es corresponde con el de la propiedad local", p0);
		}

		// <summary>
		// Retorna una cadena como: "Operación de inclusión inválida. El nivel de inclusión que intenta usar no está permitido"
		// </summary>
		internal static string InvalidIncludeOperationException
		{
			get { return "Operación de inclusión inválida. El nivel de inclusión que intenta usar no está permitido"; }
		}

		// <summary>
		// Retorna una cadena como: "El tipo de la propiedad de navegación ({0}) no es válido"
		// </summary>
		internal static string InvalidNavigationPropertyException(object p0)
		{
			return string.Format("El tipo de la propiedad de navegación ({0}) no es válido", p0);
		}

		// <summary>
		// Retorna una cadena como: "Un parámetro nulo debe ser de tipo Nullable<T>"
		// </summary>
		internal static string InvalidNullableParameterException
		{
			get { return "Un parámetro nulo debe ser de tipo Nullable<T>"; }
		}

		// <summary>
		// Retorna una cadena como: "La operación que intenta realizar no es válida sobre la entidad ({0})"
		// </summary>
		internal static string InvalidOperationEntityEntryException(object p0)
		{
			return string.Format("La operación que intenta realizar no es válida sobre la entidad ({0})", p0);
		}

		// <summary>
		// Retorna una cadena como: "Expresión de propiedad inválida"
		// </summary>
		internal static string InvalidPropertyExpressionException
		{
			get { return "Expresión de propiedad inválida"; }
		}

		// <summary>
		// Retorna una cadena como: "El tipo de dato de la propiedad llave en la entidad ({0}) no se ajusta con el tipo de llave"
		// </summary>
		internal static string InvalidTypeEntityKeyException(object p0)
		{
			return string.Format("El tipo de dato de la propiedad llave en la entidad ({0}) no se ajusta con el tipo de llave", p0);
		}

		// <summary>
		// Retorna una cadena como: "La propiedad de navegación ({0}) no existe en la entidad ({1})"
		// </summary>
		internal static string NavigationPropertyDontExistsException(object p0, object p1)
		{
			return string.Format("La propiedad de navegación ({0}) no existe en la entidad ({1})", p0, p1);
		}

		// <summary>
		// Retorna una cadena como: "El repositorio {0} no ha sido inicializado"
		// </summary>
		internal static string NotInitializedRepositoryException(object p0)
		{
			return string.Format("El repositorio {0} no ha sido inicializado", p0);
		}

		// <summary>
		// Retorna una cadena como: "El parámetro ({0}) no ha sido definido"
		// </summary>
		internal static string UndefinedParameterException(object p0)
		{
			return string.Format("El parámetro ({0}) no ha sido definido", p0);
		}

		// <summary>
		// Retorna una cadena como: "Expresión no soportada ({0})"
		// </summary>
		internal static string UnsupportedExpressionException(object p0)
		{
			return string.Format("Expresión no soportada ({0})", p0);
		}
	}

	// <summary>
	// Fabrica de excepciones fuertemente tipadas.
	// </summary>
	[GeneratedCode("Resources.tt", "1.0.0.0")]
	internal static class Error
	{
		// <summary>
		// ArgumentException con un mensaje como: "El parámetro ({0}) no tiene un valor válido"
		// </summary>
		internal static Exception ArgumentException(object p0)
		{
			return new ArgumentException(Strings.ArgumentException(p0));
		}

		// <summary>
		// EntityAlreadyExistsException con un mensaje como: "Está intentando agregar una entidad ({0}) que ya existe en el contexto de datos"
		// </summary>
		internal static Exception EntityAlreadyExistsException(object p0)
		{
			return new EntityAlreadyExistsException(Strings.EntityAlreadyExistsException(p0));
		}

		// <summary>
		// ForeignPropertyDontExistsException con un mensaje como: "La propiedad foránea ({0}) no existe en la entidad ({1})"
		// </summary>
		internal static Exception ForeignPropertyDontExistsException(object p0, object p1)
		{
			return new ForeignPropertyDontExistsException(Strings.ForeignPropertyDontExistsException(p0, p1));
		}

		// <summary>
		// InvalidEntityKeyException con un mensaje como: "La entidad ({0}) tiene una cantidad inválida de propiedades marcadas como llave principal"
		// </summary>
		internal static Exception InvalidEntityKeyException(object p0)
		{
			return new InvalidEntityKeyException(Strings.InvalidEntityKeyException(p0));
		}

		// <summary>
		// InvalidForeignPropertyException con un mensaje como: "El tipo de la propiedad de foránea ({0}) no es corresponde con el de la propiedad local"
		// </summary>
		internal static Exception InvalidForeignPropertyException(object p0)
		{
			return new InvalidForeignPropertyException(Strings.InvalidForeignPropertyException(p0));
		}

		// <summary>
		// InvalidIncludeOperationException con un mensaje como: "Operación de inclusión inválida. El nivel de inclusión que intenta usar no está permitido"
		// </summary>
		internal static Exception InvalidIncludeOperationException()
		{
			return new InvalidIncludeOperationException(Strings.InvalidIncludeOperationException);
		}

		// <summary>
		// InvalidNavigationPropertyException con un mensaje como: "El tipo de la propiedad de navegación ({0}) no es válido"
		// </summary>
		internal static Exception InvalidNavigationPropertyException(object p0)
		{
			return new InvalidNavigationPropertyException(Strings.InvalidNavigationPropertyException(p0));
		}

		// <summary>
		// InvalidNullableParameterException con un mensaje como: "Un parámetro nulo debe ser de tipo Nullable<T>"
		// </summary>
		internal static Exception InvalidNullableParameterException()
		{
			return new InvalidNullableParameterException(Strings.InvalidNullableParameterException);
		}

		// <summary>
		// InvalidOperationEntityEntryException con un mensaje como: "La operación que intenta realizar no es válida sobre la entidad ({0})"
		// </summary>
		internal static Exception InvalidOperationEntityEntryException(object p0)
		{
			return new InvalidOperationEntityEntryException(Strings.InvalidOperationEntityEntryException(p0));
		}

		// <summary>
		// InvalidPropertyExpressionException con un mensaje como: "Expresión de propiedad inválida"
		// </summary>
		internal static Exception InvalidPropertyExpressionException()
		{
			return new InvalidPropertyExpressionException(Strings.InvalidPropertyExpressionException);
		}

		// <summary>
		// InvalidTypeEntityKeyException con un mensaje como: "El tipo de dato de la propiedad llave en la entidad ({0}) no se ajusta con el tipo de llave"
		// </summary>
		internal static Exception InvalidTypeEntityKeyException(object p0)
		{
			return new InvalidTypeEntityKeyException(Strings.InvalidTypeEntityKeyException(p0));
		}

		// <summary>
		// NavigationPropertyDontExistsException con un mensaje como: "La propiedad de navegación ({0}) no existe en la entidad ({1})"
		// </summary>
		internal static Exception NavigationPropertyDontExistsException(object p0, object p1)
		{
			return new NavigationPropertyDontExistsException(Strings.NavigationPropertyDontExistsException(p0, p1));
		}

		// <summary>
		// NotInitializedRepositoryException con un mensaje como: "El repositorio {0} no ha sido inicializado"
		// </summary>
		internal static Exception NotInitializedRepositoryException(object p0)
		{
			return new NotInitializedRepositoryException(Strings.NotInitializedRepositoryException(p0));
		}

		// <summary>
		// UndefinedParameterException con un mensaje como: "El parámetro ({0}) no ha sido definido"
		// </summary>
		internal static Exception UndefinedParameterException(object p0)
		{
			return new UndefinedParameterException(Strings.UndefinedParameterException(p0));
		}

		// <summary>
		// UnsupportedExpressionException con un mensaje como: "Expresión no soportada ({0})"
		// </summary>
		internal static Exception UnsupportedExpressionException(object p0)
		{
			return new UnsupportedExpressionException(Strings.UnsupportedExpressionException(p0));
		}

		// <summary>
		// The exception that is thrown when the value of an argument is outside the allowable range of values as defined by the invoked method.
		// </summary>
		internal static Exception ArgumentOutOfRange(string paramName)
		{
			return new ArgumentOutOfRangeException(paramName);
		}

		// <summary>
		// The exception that is thrown when the author has yet to implement the logic at this point in the program. This can act as an exception based TODO tag.
		// </summary>
		internal static Exception NotImplemented()
		{
			return new NotImplementedException();
		}

		// <summary>
		// The exception that is thrown when an invoked method is not supported, or when there is an attempt to
		// read, seek, or write to a stream that does not support the invoked functionality.
		// </summary>
		internal static Exception NotSupported()
		{
			return new NotSupportedException();
		}
	}
}
