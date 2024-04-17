﻿using Dapper;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Models.Servicios
{

	public interface IRepositoriosTiposCuentas
	{
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Crear(TipoCuenta tipoCuenta);
		Task<bool> Existe(string nombre, int usuarioId);
		Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtnerPorId(int id, int usuarioId);
    }
	public class RepositoriosTiposCuentas: IRepositoriosTiposCuentas
	{
		private readonly string connectionString;
        public RepositoriosTiposCuentas(IConfiguration configuration)
        {
			connectionString = configuration.GetConnectionString("DefaultConnection");
            
        }

		public async Task Crear(TipoCuenta tipoCuenta)
		{
			using var connection = new SqlConnection(connectionString);
			var id = await connection.QuerySingleAsync<int>(@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden) VALUES (@Nombre, @UsuarioId, 0);SELECT SCOPE_IDENTITY();", tipoCuenta);
			tipoCuenta.Id= id;
		}

		public async Task<bool> Existe(string nombre, int usuarioId)
		{
			using var connection = new SqlConnection(connectionString);
			var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas WHERE Nombre=@Nombre AND UsuarioId = @UsuarioId;", new {nombre, usuarioId});
			return existe == 1;
		}


		public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
		{

			using var connection = new SqlConnection(connectionString);
			return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden FROM TiposCuentas WHERE UsuarioId = @UsuarioId;", new {usuarioId});
		}

		public async Task Actualizar(TipoCuenta tipoCuenta)
		{
			using var connection = new SqlConnection(connectionString);
			await connection.ExecuteAsync(@"UPDATE TiposCuentas SET Nombre = @Nombre WHERE UsuarioId= @UsuarioId", tipoCuenta);
		}

		public async Task<TipoCuenta> ObtnerPorId(int id, int usuarioId)
		{
			using var connection = new SqlConnection(connectionString);
			return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden FROM TiposCuentas WHERE Id = @Id AND UsuarioId = @UsuarioId;", new {id, usuarioId});
		}
    }
}
