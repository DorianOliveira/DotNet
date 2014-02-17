/*
 
 * ConvertTo - Generic conversion class version 1.0
 * author: Dorian de Oliveira Torres
 * dorian_oliveira@yahoo.com.br
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;

namespace Conversion
{
    /// <summary>
    /// Generic class tools to convert objects to any type
    /// </summary>
    public static class ConvertAll
    {
        #region Get Collection

        /// <summary>
        /// Convert obj to T type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static T GetCollection<T>(ICollection obj)
        {
            //If T type is array
            if (typeof(T).IsArray)
            {
                ICollection collection = (ICollection)Activator.CreateInstance(typeof(T), obj.Count);

                obj.CopyTo((Array)collection, 0);

                return (T)collection;
            }
            return (T)Activator.CreateInstance(typeof(T), obj);
        } 

        #endregion

        /// <summary>
        /// Convert obj to T type
        /// </summary>
        /// <typeparam name="T">Generic type T</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object obj)
        {
            #region Variable

            Type typeObj;
            Type typeNullable;
            Type typeString;
            Type typeUnderlying;
            Type typeT = typeof(T);
            T emptyInstance = default(T);
            T defaultValue = default(T);

            #endregion

            try
            {
                #region Init variables

                defaultValue = default(T);

                typeNullable = typeof(Nullable<>);
                typeString = typeof(string);
                typeUnderlying = typeT;

                typeObj = null;
                if (obj != null)
                    typeObj = obj.GetType();

                //In case of nullable types, the type to do conversion is it underlying type
                //Ex: "Nullable<int>", "int?" returns "int" type.
                if (typeT.IsGenericType && typeT.GetGenericTypeDefinition() == typeNullable)
                    typeUnderlying = Nullable.GetUnderlyingType(typeUnderlying);

                try
                {
                    emptyInstance = (T)Activator.CreateInstance(typeT, 0);
                }
                catch
                {
                }

                #endregion

                #region Strings

                if (typeT == typeString)
                {
                    try
                    {
                        //Null values or empty strings, returns an empty string
                        if (obj == null || (obj != null && obj.GetType() == typeString && string.IsNullOrEmpty(obj.ToString())))
                            return (T)Convert.ChangeType(string.Empty, typeString);

                        //Try convert obj to string
                        return (T)Convert.ChangeType(obj, typeString);
                    }
                    catch
                    {
                        //On error, returns a empty string
                        return (T)Convert.ChangeType(string.Empty, typeString);
                    }
                }

                #endregion

                #region Null values (Nullable<>)

                //In case obj is a null value, returns the default value
                if (obj == null || obj == DBNull.Value)
                {
                    if (emptyInstance is ICollection)
                        return emptyInstance;
                    return defaultValue;
                }

                #endregion

                #region Collections

                //Convert collection to collection
                if (emptyInstance is ICollection && obj is ICollection)
                {
                    if (typeT == typeObj)
                        return (T)obj;
                    return (T)GetCollection<T>((ICollection)obj);
                }

                #endregion

                //Convert value
                return (T)Convert.ChangeType(obj, typeUnderlying);
            }
            catch
            {
                if (emptyInstance is ICollection)
                    return emptyInstance;
                //Any error, returns default value from T
                return defaultValue;
            }
        }
    }
}
