using Microsoft.EntityFrameworkCore;
using NIA_CRM.Models;
using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace NIA_CRM.Data
{
    public class NIAEmailInitializerClass
    {
        public static void Initialize(IServiceProvider serviceProvider,
            bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true)
        {
            using (var context = new NIACRMContext(
                serviceProvider.GetRequiredService<DbContextOptions<NIACRMContext>>()))
            {
                //Refresh the database as per the parameter options
                #region Prepare the Database
                try
                {
                    //Note: .CanConnect() will return false if the database is not there!
                    if (DeleteDatabase || !context.Database.CanConnect())
                    {
                        context.Database.EnsureDeleted(); //Delete the existing version 
                        if (UseMigrations)
                        {
                            context.Database.Migrate(); //Create the Database and apply all migrations
                        }
                        else
                        {
                            context.Database.EnsureCreated(); //Create and update the database as per the Model
                        }

                    }
                    else //The database is already created
                    {
                        if (UseMigrations)
                        {
                            context.Database.Migrate(); //Apply all migrations
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
                #endregion

                //Seed data needed for production and during development
                #region Seed Required Data
                try
                {
                    if (!context.ProductionEmails.Any())
                    {
                        // Random member names to be used in the email content
                        var randomNames = new List<string>
        {
            "John Doe", "Jane Smith", "Robert Johnson", "Emily Davis", "Michael Brown"
        };

                        // Seeding predefined production email templates with random member names
                        context.ProductionEmails.AddRange(
                             new ProductionEmail
                             {
                                 Id = 1,
                                 EmailType = "Welcome Email",
                                 Subject = "Welcome to NIA!",
                                 Body = $"Dear {randomNames[0]},\n\nWelcome to the NIA community! We are thrilled to have you onboard. Please let us know if you need any assistance.\n\nBest regards,\nNIA Team",
                                 //IsActive = true
                             },
                             new ProductionEmail
                             {
                                 Id = 2,
                                 EmailType = "Renewal Reminder",
                                 Subject = "Membership Renewal Reminder",
                                 Body = $"Dear {randomNames[1]},\n\nYour membership with NIA is about to expire on 2025-02-15. Please renew your membership to continue enjoying all the benefits.\n\nBest regards,\nNIA Team",
                                 //IsActive = true
                             },
                             new ProductionEmail
                             {
                                 Id = 3,
                                 EmailType = "Cancellation Notice",
                                 Subject = "Membership Cancellation Confirmation",
                                 Body = $"Dear {randomNames[2]},\n\nWe are sorry to see you go. Your membership has been successfully canceled. If you change your mind in the future, we’d love to have you back.\n\nBest regards,\nNIA Team",
                                 //IsActive = true
                             },
                             new ProductionEmail
                             {
                                 Id = 4,
                                 EmailType = "Membership Update",
                                 Subject = "Important Membership Update",
                                 Body = $"Dear {randomNames[3]},\n\nWe would like to inform you of an important update regarding your membership status. Please log in to your account for more details.\n\nBest regards,\nNIA Team",
                                 //IsActive = true
                             },
                             new ProductionEmail
                             {
                                 Id = 5,
                                 EmailType = "General Notification",
                                 Subject = "NIA System Update",
                                 Body = $"Dear {randomNames[4]},\n\nThis is a notification regarding a recent update to the NIA system. We encourage you to check out the new features and improvements.\n\nBest regards,\nNIA Team",
                                 //IsActive = true
                             });
                        context.SaveChanges();


                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);

                }
                #endregion

            }
        }
    }
}