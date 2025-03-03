using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Data;

namespace Persistence
{
    public class DatabaseSchemaValidator
    {
        private readonly AppDbContext _dbContext;

        public DatabaseSchemaValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void ValidateSchema()
        {
            try
            {
                // 1️⃣ Verifica si la base de datos es accesible
                if (!_dbContext.Database.CanConnect())
                {
                    throw new Exception("⚠ ERROR: No se pudo conectar a la base de datos.");
                }

                // 2️⃣ Obtiene las entidades del modelo en C#
                var entityTypes = _dbContext.Model.GetEntityTypes().Select(e => e.GetTableName()).ToList();

                // 3️⃣ Obtiene las tablas existentes en la base de datos
                var tablesInDatabase = _dbContext.Database
                    .GetDbConnection()
                    .GetSchema("Tables") // Obtiene todas las tablas
                    .Rows
                    .Cast<DataRow>()
                    .Select(row => row["TABLE_NAME"].ToString())
                    .ToList();

                // 4️⃣ Valida que las entidades en C# coincidan con las tablas en la BD
                var missingTables = entityTypes.Except(tablesInDatabase).ToList();

                if (missingTables.Any())
                {
                    throw new Exception($"⚠ ERROR: Las siguientes tablas no existen en la BD, pero sí en el modelo: {string.Join(", ", missingTables)}");
                }

                Console.WriteLine("✅ La base de datos coincide con los modelos en C#.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error de validación del esquema: {ex.Message}");
            }
        }
    }
}
