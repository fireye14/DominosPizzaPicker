using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using DominosPizzaPicker.Backend.DataObjects;
using DominosPizzaPicker.Backend.Models;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using Owin;
using System.Data.SqlTypes;

namespace DominosPizzaPicker.Backend
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration().UseDefaultConfiguration().ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            //Database.SetInitializer(new DominosPizzaPickerInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            Database.SetInitializer<DominosPizzaPickerContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class DominosPizzaPickerInitializer : CreateDatabaseIfNotExists<DominosPizzaPickerContext>
    {
        // helper
        protected string GetNewGuid => Guid.NewGuid().ToString();        

        protected override void Seed(DominosPizzaPickerContext context)
        {
            
            ///
            ///  Insert Sauces
            /// 

            List<Sauce> SauceList = new List<Sauce>
            {
                // Needs Id defined for some reason, even though Id will have a default value of NewId() in SQL
                new Sauce {Id = Guid.NewGuid().ToString(), Name = "Alfredo Sauce"},
                new Sauce {Id = Guid.NewGuid().ToString(), Name = "BBQ Sauce"},
                new Sauce {Id = Guid.NewGuid().ToString(), Name = "Garlic Parmesan White Sauce"},
                new Sauce {Id = Guid.NewGuid().ToString(), Name = "Hearty Marinara Sauce"},
                new Sauce {Id = Guid.NewGuid().ToString(), Name = "Robust Inspired Tomato Sauce"},
            };

            context.Sauces.AddRange(SauceList);

            
            /// 
            /// Insert Toppings
            /// 

            List<Topping> ToppingList = new List<Topping>
            {
                new Topping{Id = GetNewGuid, Name = "Bacon",                     IsMeat = true,  IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Banana Peppers",            IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Beef",                      IsMeat = true,  IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Black Olives",              IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Cheddar Cheese",            IsMeat = false, IsCheese = true },
                new Topping{Id = GetNewGuid, Name = "Diced Tomatoes",            IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Feta Cheese",               IsMeat = false, IsCheese = true },
                new Topping{Id = GetNewGuid, Name = "Green Olives",              IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Green Peppers",             IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Ham",                       IsMeat = true,  IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Hot Sauce",                 IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Italian Sausage",           IsMeat = true,  IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Jalapeno Peppers",          IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Mushrooms",                 IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Onion",                     IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Pepperoni",                 IsMeat = true,  IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Philly Steak",              IsMeat = true,  IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Pineapple",                 IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Premium Chicken",           IsMeat = true,  IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Roasted Red Peppers",       IsMeat = false, IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Salami",                    IsMeat = true,  IsCheese = false},
                new Topping{Id = GetNewGuid, Name = "Shredded Parmesan Asiago",  IsMeat = false, IsCheese = true },
                new Topping{Id = GetNewGuid, Name = "Shredded Provolone Cheese", IsMeat = false, IsCheese = true },
                new Topping{Id = GetNewGuid, Name = "Spinach",                   IsMeat = false, IsCheese = false},
            };


            context.Toppings.AddRange(ToppingList);


            ///
            /// Insert Pizzas
            /// 

            var PizzaList = new List<Pizza>();

            SauceList.ForEach(s =>
                // sauce

                ToppingList.ForEach(t1 =>
                    // topping1

                    ToppingList.Where(x => !x.Id.Equals(t1.Id)).ToList().ForEach(t2 =>
                        // topping2 - can't equal first topping

                        ToppingList.Where(x => !x.Id.Equals(t1.Id) && !x.Id.Equals(t2.Id)).ToList().ForEach(t3 =>
                        // topping3 - can't equal any other topping
                        {
                            Pizza newPizza = new Pizza { Id = GetNewGuid, SauceId = s.Id, Topping1Id = t1.Id, Topping2Id = t2.Id, Topping3Id = t3.Id, Eaten = false, DateEaten = (DateTime)SqlDateTime.MinValue, Rating = 0f, Comment = string.Empty };
                            if (!PizzaList.Contains(newPizza))
                            {
                                PizzaList.Add(newPizza);                                
                            }

                        }
                ))));
           
            context.Pizzas.AddRange(PizzaList);



            base.Seed(context);
        }


    }
}

