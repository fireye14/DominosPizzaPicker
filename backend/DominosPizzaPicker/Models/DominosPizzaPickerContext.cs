using DominosPizzaPicker.Backend.DataObjects;
using Microsoft.Azure.Mobile.Server.Tables;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace DominosPizzaPicker.Backend.Models
{
    public class DominosPizzaPickerContext : DbContext
    {
        // connection string set in Web.config
        private const string connectionStringName = "Name=PizzaPickerConnectionString";

        public DominosPizzaPickerContext() : base(connectionStringName)
        {
        }

        // 
        public DbSet<Sauce> Sauces { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<PizzaView> PizzaView { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            // Manually set table name instead of adding s to the end of it
            modelBuilder.Entity<PizzaView>().ToTable("PizzaView");
        }
    }
}