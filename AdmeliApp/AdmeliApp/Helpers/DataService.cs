using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Helpers
{
    public class DataService
    {
        public Response InsertPersonal(Personal personal)
        {
            try
            {
                using(DataAccess da = new DataAccess())
                {
                    /// Verificando si hay usuario logueado
                    Personal OldPersonal = da.First<Personal>();
                    if (OldPersonal != null)
                    {
                        da.Delete(OldPersonal);
                    }

                    /// Insertando un nuevo usuario para recordar
                    da.Insert(personal);
                }

                /// Respuesta cuando todo esta correcto
                return new Response()
                {
                    Message = "Personal guardado OK",
                    IsSuccess = true,
                    Result = personal,
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    IsSuccess = false,
                };
            }
        }

        public Personal GetPersonal()
        {
            using(DataAccess da = new DataAccess())
            {
                return da.First<Personal>();
            }
        }

        internal void DeletePersonal(Personal personal)
        {
            using(DataAccess da = new DataAccess())
            {
                da.Delete(personal);
            }
        }
    }
}
