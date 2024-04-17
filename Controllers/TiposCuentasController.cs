using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Models.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController: Controller
    {
        private readonly string connectionString;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositoriosTiposCuentas repositoriosTiposCuentas;
        

        //ejemplo para validar connexion a bd con sql server
        //public TiposCuentasController(IConfiguration configuration)
        //{

        //    connectionString = configuration.GetConnectionString("DefaultConnection");
        //}

        public TiposCuentasController(IServicioUsuarios servicioUsuarios, IRepositoriosTiposCuentas repositoriosTiposCuentas )
        {
			this.repositoriosTiposCuentas = repositoriosTiposCuentas;
            this.servicioUsuarios = servicioUsuarios;
        }
        public IActionResult Crear()
        {
            //using (var connection = new SqlConnection(connectionString))
            //{
            //    var query = connection.Query("SELECT 1").FirstOrDefault();           
            //}
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            tipoCuenta.UsuarioId = servicioUsuarios.ObtenerUsuarioId();
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            //entorno de prueba
           
            var yaExisteTipoCuenta = await repositoriosTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            if (yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }
            await repositoriosTiposCuentas.Crear(tipoCuenta);
            //return View();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositoriosTiposCuentas.ObtnerPorId(id, usuarioId);
            if(tipoCuenta is null) //si el usuario no tiene permisos para actualizar el registro
            {
                return RedirectToAction("No encontrado", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipocuentaexiste = await repositoriosTiposCuentas.ObtnerPorId(tipoCuenta.Id, usuarioId);

            if(tipocuentaexiste is null)
            {
                return RedirectToAction("elemento no encontrado", "Home");

            }
            await repositoriosTiposCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId(); ;
            var tiposCuentas = await repositoriosTiposCuentas.Obtener(usuarioId);
            return View(tiposCuentas);
        }

        //funcion que permite validar si el tipo cuenta existe sin tener que usar el boton, esto con una funcion JS
        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await repositoriosTiposCuentas.Existe(nombre, usuarioId);
            if (yaExisteTipoCuenta)
            {
                //convertir en formato json el nombre
                return Json($"El nombre {nombre} ya existe, use otro");
            }

            return Json(true);
        }
    }
}
