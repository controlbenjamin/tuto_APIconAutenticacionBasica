using APIconAutenticacionBasica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIconAutenticacionBasica.Services
{
    public class UsuariosDAL
    {
        public static Usuarios GetUser(string username, string password)
        {
            var user = new Usuarios();

            // bcorrea@gmail.com:Pass1234
            // en base 64 es YmNvcnJlYUBnbWFpbC5jb206UGFzczEyMzQ=
            user.Username = "bcorrea@gmail.com";
            user.Password = "Pass1234";

            if (username == user.Username && password == user.Password)
            {
                return user;
            }
            else
            {
                return null;
            }

        }
    }
}
