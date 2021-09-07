namespace DominosPizzaPicker.Backend.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DominosPizzaPicker.Backend.Models.DominosPizzaPickerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "DominosPizzaPicker.Backend.Models.DominosPizzaPickerContext";
        }

        protected override void Seed(DominosPizzaPicker.Backend.Models.DominosPizzaPickerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
