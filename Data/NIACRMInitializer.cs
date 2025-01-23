using NIA_CRM.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace NIA_CRM.Data
{
    public static class NIACRMInitializer
    {
        /// <summary>
        /// Prepares the Database and seeds data as required
        /// </summary>
        /// <param name="serviceProvider">DI Container</param>
        /// <param name="DeleteDatabase">Delete the database and start from scratch</param>
        /// <param name="UseMigrations">Use Migrations or EnsureCreated</param>
        /// <param name="SeedSampleData">Add optional sample data</param>
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
                        //Now create any additional database objects such as Triggers or Views
                        //--------------------------------------------------------------------
                        //Create the Triggers
                        string sqlCmd = @"
                            CREATE TRIGGER SetProductionEmailTimestampOnUpdate
                            AFTER UPDATE ON ProductionEmail
                            BEGIN
                                UPDATE ProductionEmail
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetProductionEmailTimestampOnInsert
                            AFTER INSERT ON ProductionEmails
                            BEGIN
                                UPDATE ProductionEmail
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);
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

                /* try
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

                     //Now create any additional database objects such as Triggers or Views
                     //--------------------------------------------------------------------

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
                 }*/

                try
                {
                    //Add some Class Start times
                    if (!context.Industries.Any())
                    {
                        context.Industries.AddRange(
                            new Industry { ID = 1, IndustryName = "Alpha Steel" },
                            new Industry { ID = 2, IndustryName = "TISCO CO." },
                            new Industry { ID = 3, IndustryName = "M Time Irons" },
                            new Industry { ID = 4, IndustryName = "Forge & Foundry Inc." },
                            new Industry { ID = 5, IndustryName = "Northern Metalworks" },
                            new Industry { ID = 6, IndustryName = "Titanium Solutions" },
                            new Industry { ID = 7, IndustryName = "Phoenix Alloys" },
                            new Industry { ID = 8, IndustryName = "Galaxy Metals" },
                            new Industry { ID = 9, IndustryName = "Ironclad Industries" },
                            new Industry { ID = 10, IndustryName = "Silverline Fabrication" },
                            new Industry { ID = 11, IndustryName = "Star Steelworks" },
                            new Industry { ID = 12, IndustryName = "Summit Metal Co." },
                            new Industry { ID = 13, IndustryName = "Everest Iron Corp." },
                            new Industry { ID = 14, IndustryName = "Prime Alloy Coatings" },
                            new Industry { ID = 15, IndustryName = "Magnum Steel Solutions" },
                            new Industry { ID = 16, IndustryName = "Quantum Tech Innovations" },
                            new Industry { ID = 17, IndustryName = "Aurora Renewable Energy" },
                            new Industry { ID = 18, IndustryName = "Vertex Financial Group" },
                            new Industry { ID = 19, IndustryName = "Nova Biotech Labs" },
                            new Industry { ID = 20, IndustryName = "Summit Construction Co." },
                            new Industry { ID = 21, IndustryName = "Oceanic Shipping Corp" },
                            new Industry { ID = 22, IndustryName = "Evergreen Agriculture" },
                            new Industry { ID = 23, IndustryName = "Ironclad Manufacturing Ltd." },
                            new Industry { ID = 24, IndustryName = "Skyline Architects Inc." },
                            new Industry { ID = 25, IndustryName = "Pinnacle Consulting Services" },
                            new Industry { ID = 26, IndustryName = "Crystal Water Solutions" },
                            new Industry { ID = 27, IndustryName = "Elite Healthcare Partners" },
                            new Industry { ID = 28, IndustryName = "Galaxy IT Solutions" },
                            new Industry { ID = 29, IndustryName = "Urban Infrastructure Group" },
                            new Industry { ID = 30, IndustryName = "Horizon Aerospace Inc." },
                            new Industry { ID = 31, IndustryName = "Cobalt Mining Ventures" },
                            new Industry { ID = 32, IndustryName = "Lakeside Resorts and Hotels" },
                            new Industry { ID = 33, IndustryName = "NextGen Media Productions" },
                            new Industry { ID = 34, IndustryName = "Crestwood Pharmaceutical" },
                            new Industry { ID = 35, IndustryName = "Dynamic Logistics Group" },
                            new Industry { ID = 36, IndustryName = "Northern Timber Products" },
                            new Industry { ID = 37, IndustryName = "Brightline Education Systems" },
                            new Industry { ID = 38, IndustryName = "Fusion Energy Solutions" },
                            new Industry { ID = 39, IndustryName = "Trailblazer Automotive Group" },
                            new Industry { ID = 40, IndustryName = "Harvest Foods International" },
                            new Industry { ID = 41, IndustryName = "Regal Entertainment Network" },
                            new Industry { ID = 42, IndustryName = "EcoSmart Waste Management" },
                            new Industry { ID = 43, IndustryName = "Summit Legal Services" },
                            new Industry { ID = 44, IndustryName = "Zenith Apparel Ltd." },
                            new Industry { ID = 45, IndustryName = "BlueWave Software Inc." }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Organizations.Any())
                    {
                        context.Organizations.AddRange(
                            new Organization
                            {
                                ID = 1,
                                OrganizationName = "Steel Works Ltd.",
                                OrganizationSize = 150,
                                OrganizationWeb = "https://www.steelworksltd.com",
                                IndustryID = 1 // Related to "Alpha Steel"
                            },
                            new Organization
                            {
                                ID = 2,
                                OrganizationName = "TISCO Technologies",
                                OrganizationSize = 200,
                                OrganizationWeb = "https://www.tiscotech.com",
                                IndustryID = 2 // Related to "TISCO CO."
                            },
                            new Organization
                            {
                                ID = 3,
                                OrganizationName = "Iron Builders Inc.",
                                OrganizationSize = 300,
                                OrganizationWeb = "https://www.ironbuilders.com",
                                IndustryID = 3 // Related to "M Time Irons"
                            },
                            new Organization
                            {
                                ID = 4,
                                OrganizationName = "Forge Foundry Co.",
                                OrganizationSize = 100,
                                OrganizationWeb = "https://www.forgefoundry.com",
                                IndustryID = 4 // Related to "Forge & Foundry Inc."
                            },
                            new Organization
                            {
                                ID = 5,
                                OrganizationName = "Northern Metal Solutions",
                                OrganizationSize = 180,
                                OrganizationWeb = "https://www.northernmetals.com",
                                IndustryID = 5 // Related to "Northern Metalworks"
                            },
                            new Organization
                            {
                                ID = 6,
                                OrganizationName = "Titanium Group Ltd.",
                                OrganizationSize = 250,
                                OrganizationWeb = "https://www.titaniumgroup.com",
                                IndustryID = 6 // Related to "Titanium Solutions"
                            },
                            new Organization
                            {
                                ID = 7,
                                OrganizationName = "Phoenix Alloy Works",
                                OrganizationSize = 320,
                                OrganizationWeb = "https://www.phoenixalloy.com",
                                IndustryID = 7 // Related to "Phoenix Alloys"
                            },
                            new Organization
                            {
                                ID = 8,
                                OrganizationName = "Galaxy Fabricators",
                                OrganizationSize = 190,
                                OrganizationWeb = "https://www.galaxyfabricators.com",
                                IndustryID = 8 // Related to "Galaxy Metals"
                            },
                            new Organization
                            {
                                ID = 9,
                                OrganizationName = "Ironclad Enterprises",
                                OrganizationSize = 400,
                                OrganizationWeb = "https://www.ironcladent.com",
                                IndustryID = 9 // Related to "Ironclad Industries"
                            },
                            new Organization
                            {
                                ID = 10,
                                OrganizationName = "Silverline Engineering",
                                OrganizationSize = 220,
                                OrganizationWeb = "https://www.silverlineeng.com",
                                IndustryID = 10 // Related to "Silverline Fabrication"
                            },
                            new Organization
                            {
                                ID = 11,
                                OrganizationName = "Star Steel Corp.",
                                OrganizationSize = 500,
                                OrganizationWeb = "https://www.starsteelcorp.com",
                                IndustryID = 11 // Related to "Star Steelworks"
                            },
                            new Organization
                            {
                                ID = 12,
                                OrganizationName = "Summit Iron & Steel",
                                OrganizationSize = 350,
                                OrganizationWeb = "https://www.summitironsteel.com",
                                IndustryID = 12 // Related to "Summit Metal Co."
                            },
                            new Organization
                            {
                                ID = 13,
                                OrganizationName = "Everest Metal Industries",
                                OrganizationSize = 280,
                                OrganizationWeb = "https://www.everestmetals.com",
                                IndustryID = 13 // Related to "Everest Iron Corp."
                            },
                            new Organization
                            {
                                ID = 14,
                                OrganizationName = "Prime Alloy Co.",
                                OrganizationSize = 330,
                                OrganizationWeb = "https://www.primealloy.com",
                                IndustryID = 14 // Related to "Prime Alloy Coatings"
                            },
                            new Organization
                            {
                                ID = 15,
                                OrganizationName = "Magnum Steelworks",
                                OrganizationSize = 420,
                                OrganizationWeb = "https://www.magnumsteelworks.com",
                                IndustryID = 15 // Related to "Magnum Steel Solutions"
                            },
                            new Organization
                            {
                                ID = 16,
                                OrganizationName = "Quantum Tech Systems",
                                OrganizationSize = 300,
                                OrganizationWeb = "https://www.quantumtechsystems.com",
                                IndustryID = 16
                            },
                            new Organization
                            {
                                ID = 17,
                                OrganizationName = "Aurora Solar Energy",
                                OrganizationSize = 150,
                                OrganizationWeb = "https://www.aurorasolarenergy.com",
                                IndustryID = 17
                            },
                            new Organization
                            {
                                ID = 18,
                                OrganizationName = "Vertex Finance Group",
                                OrganizationSize = 500,
                                OrganizationWeb = "https://www.vertexfinancegroup.com",
                                IndustryID = 18
                            },
                            new Organization
                            {
                                ID = 19,
                                OrganizationName = "Nova Biotech Solutions",
                                OrganizationSize = 240,
                                OrganizationWeb = "https://www.novabiotechsolutions.com",
                                IndustryID = 19
                            },
                            new Organization
                            {
                                ID = 20,
                                OrganizationName = "Summit Builders Ltd.",
                                OrganizationSize = 600,
                                OrganizationWeb = "https://www.summitbuilders.com",
                                IndustryID = 20
                            },
                            new Organization
                            {
                                ID = 21,
                                OrganizationName = "Oceanic Maritime Inc.",
                                OrganizationSize = 1200,
                                OrganizationWeb = "https://www.oceanicmaritime.com",
                                IndustryID = 21
                            },
                            new Organization
                            {
                                ID = 22,
                                OrganizationName = "Evergreen AgroTech",
                                OrganizationSize = 320,
                                OrganizationWeb = "https://www.evergreenagrotech.com",
                                IndustryID = 22
                            },
                            new Organization
                            {
                                ID = 23,
                                OrganizationName = "Ironclad Metalworks",
                                OrganizationSize = 450,
                                OrganizationWeb = "https://www.ironcladmetalworks.com",
                                IndustryID = 23
                            },
                            new Organization
                            {
                                ID = 24,
                                OrganizationName = "Skyline Design Studios",
                                OrganizationSize = 110,
                                OrganizationWeb = "https://www.skylinedesignstudios.com",
                                IndustryID = 24
                            },
                            new Organization
                            {
                                ID = 25,
                                OrganizationName = "Pinnacle Business Advisors",
                                OrganizationSize = 90,
                                OrganizationWeb = "https://www.pinnacleadvisors.com",
                                IndustryID = 25
                            },
                            new Organization
                            {
                                ID = 26,
                                OrganizationName = "Crystal Pure Water Inc.",
                                OrganizationSize = 230,
                                OrganizationWeb = "https://www.crystalpurewater.com",
                                IndustryID = 26
                            },
                            new Organization
                            {
                                ID = 27,
                                OrganizationName = "Elite Health Systems",
                                OrganizationSize = 700,
                                OrganizationWeb = "https://www.elitehealthsystems.com",
                                IndustryID = 27
                            },
                            new Organization
                            {
                                ID = 28,
                                OrganizationName = "Galaxy IT Hub",
                                OrganizationSize = 350,
                                OrganizationWeb = "https://www.galaxyithub.com",
                                IndustryID = 28
                            },
                            new Organization
                            {
                                ID = 29,
                                OrganizationName = "Urban InfraWorks",
                                OrganizationSize = 520,
                                OrganizationWeb = "https://www.urbaninfraworks.com",
                                IndustryID = 29
                            },
                            new Organization
                            {
                                ID = 30,
                                OrganizationName = "Horizon AeroTech",
                                OrganizationSize = 430,
                                OrganizationWeb = "https://www.horizonaerotech.com",
                                IndustryID = 30
                            },
                            new Organization
                            {
                                ID = 31,
                                OrganizationName = "Cobalt Mining Ltd.",
                                OrganizationSize = 780,
                                OrganizationWeb = "https://www.cobaltmining.com",
                                IndustryID = 31
                            },
                            new Organization
                            {
                                ID = 32,
                                OrganizationName = "Lakeside Hospitality Group",
                                OrganizationSize = 250,
                                OrganizationWeb = "https://www.lakesidehospitality.com",
                                IndustryID = 32
                            },
                            new Organization
                            {
                                ID = 33,
                                OrganizationName = "NextGen Media Labs",
                                OrganizationSize = 180,
                                OrganizationWeb = "https://www.nextgenmedialabs.com",
                                IndustryID = 33
                            },
                            new Organization
                            {
                                ID = 34,
                                OrganizationName = "Crestwood PharmaTech",
                                OrganizationSize = 670,
                                OrganizationWeb = "https://www.crestwoodpharmatech.com",
                                IndustryID = 34
                            },
                            new Organization
                            {
                                ID = 35,
                                OrganizationName = "Dynamic Freight Solutions",
                                OrganizationSize = 400,
                                OrganizationWeb = "https://www.dynamicfreightsolutions.com",
                                IndustryID = 35
                            },
                            new Organization
                            {
                                ID = 36,
                                OrganizationName = "Northern Lumber Co.",
                                OrganizationSize = 190,
                                OrganizationWeb = "https://www.northernlumber.com",
                                IndustryID = 36
                            },
                            new Organization
                            {
                                ID = 37,
                                OrganizationName = "Brightline EduWorks",
                                OrganizationSize = 320,
                                OrganizationWeb = "https://www.brightlineeduworks.com",
                                IndustryID = 37
                            },
                            new Organization
                            {
                                ID = 38,
                                OrganizationName = "Fusion Green Energy",
                                OrganizationSize = 210,
                                OrganizationWeb = "https://www.fusiongreenenergy.com",
                                IndustryID = 38
                            },
                            new Organization
                            {
                                ID = 39,
                                OrganizationName = "Trailblazer Motors",
                                OrganizationSize = 750,
                                OrganizationWeb = "https://www.trailblazermotors.com",
                                IndustryID = 39
                            },
                            new Organization
                            {
                                ID = 40,
                                OrganizationName = "Harvest Global Foods",
                                OrganizationSize = 500,
                                OrganizationWeb = "https://www.harvestglobalfoods.com",
                                IndustryID = 40
                            },
                            new Organization
                            {
                                ID = 41,
                                OrganizationName = "Regal Movie Studios",
                                OrganizationSize = 310,
                                OrganizationWeb = "https://www.regalmoviestudios.com",
                                IndustryID = 41
                            },
                            new Organization
                            {
                                ID = 42,
                                OrganizationName = "EcoSmart Recycling Co.",
                                OrganizationSize = 275,
                                OrganizationWeb = "https://www.ecosmartrecycling.com",
                                IndustryID = 42
                            },
                            new Organization
                            {
                                ID = 43,
                                OrganizationName = "Summit Legal Partners",
                                OrganizationSize = 125,
                                OrganizationWeb = "https://www.summitlegalpartners.com",
                                IndustryID = 43
                            },
                            new Organization
                            {
                                ID = 44,
                                OrganizationName = "Zenith Fashion Ltd.",
                                OrganizationSize = 490,
                                OrganizationWeb = "https://www.zenithfashion.com",
                                IndustryID = 44
                            },
                            new Organization
                            {
                                ID = 45,
                                OrganizationName = "BlueWave Software Group",
                                OrganizationSize = 600,
                                OrganizationWeb = "https://www.bluewavesoftware.com",
                                IndustryID = 45
                            }

                        );
                        context.SaveChanges();
                    }
                    if (!context.MembershipTypes.Any())
                    {
                        context.MembershipTypes.AddRange(
                            new MembershipType
                            {
                                ID = 1,
                                TypeName = "Basic Membership",
                                TypeDescr = "Access to gym equipment and locker room facilities."
                            },
                            new MembershipType
                            {
                                ID = 2,
                                TypeName = "Premium Membership",
                                TypeDescr = "Includes Basic Membership benefits plus access to group classes and pool."
                            },
                            new MembershipType
                            {
                                ID = 3,
                                TypeName = "Family Membership",
                                TypeDescr = "Includes Premium Membership benefits for up to 4 family members."
                            },
                            new MembershipType
                            {
                                ID = 4,
                                TypeName = "Student Membership",
                                TypeDescr = "Discounted membership for students with valid ID."
                            },
                            new MembershipType
                            {
                                ID = 5,
                                TypeName = "Corporate Membership",
                                TypeDescr = "Special membership for employees of partner organizations."
                            }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Members.Any())
                    {
                        context.Members.AddRange(
                        new Member
                        {
                            ID = 1,
                            MemberFirstName = "John",
                            MemberMiddleName = null,
                            MemberLastName = "Doe",
                            JoinDate = new DateTime(2021, 6, 15),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 1, // Linked to "Steel Works Ltd."
                            Address = new Address
                            {
                                AddressLineOne = "123 Elm St",
                                AddressLineTwo = "Apt 4B",
                                City = "Metaltown",
                                StateProvince = "Metal State",
                                PostalCode = "M1E2L3",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 2,
                            MemberFirstName = "Jane",
                            MemberMiddleName = null,
                            MemberLastName = "Smith",
                            JoinDate = new DateTime(2022, 1, 20),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 2, // Linked to "TISCO Technologies"
                            Address = new Address
                            {
                                AddressLineOne = "456 Oak Ave",
                                AddressLineTwo = null,
                                City = "Ironville",
                                StateProvince = "Steel Province",
                                PostalCode = "I2R3O4",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 3,
                            MemberFirstName = "Robert",
                            MemberMiddleName = null,
                            MemberLastName = "Johnson",
                            JoinDate = new DateTime(2023, 3, 12),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 3, // Linked to "Iron Builders Inc."
                            Address = new Address
                            {
                                AddressLineOne = "789 Pine Rd",
                                AddressLineTwo = "Suite 302",
                                City = "Steelton",
                                StateProvince = "Metal Zone",
                                PostalCode = "S3T4E5",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 4,
                            MemberFirstName = "Emily",
                            MemberMiddleName = null,
                            MemberLastName = "Davis",
                            JoinDate = new DateTime(2022, 8, 8),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 4, // Linked to "Forge Foundry Co."
                            Address = new Address
                            {
                                AddressLineOne = "101 Birch Blvd",
                                AddressLineTwo = null,
                                City = "Alloy City",
                                StateProvince = "Forge Region",
                                PostalCode = "A1C2T3",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 5,
                            MemberFirstName = "Michael",
                            MemberMiddleName = "Adam",
                            MemberLastName = "Brown",
                            JoinDate = new DateTime(2020, 5, 10),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 5,
                            Address = new Address
                            {
                                AddressLineOne = "201 Maple Street",
                                AddressLineTwo = "Unit 101",
                                City = "Copperville",
                                StateProvince = "Metalland",
                                PostalCode = "C2P1E1",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 6,
                            MemberFirstName = "Sarah",
                            MemberMiddleName = null,
                            MemberLastName = "Johnson",
                            JoinDate = new DateTime(2021, 9, 15),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 6,
                            Address = new Address
                            {
                                AddressLineOne = "543 Cedar Lane",
                                AddressLineTwo = null,
                                City = "Ironcrest",
                                StateProvince = "Steelstate",
                                PostalCode = "I3N4O5",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 7,
                            MemberFirstName = "William",
                            MemberMiddleName = null,
                            MemberLastName = "Taylor",
                            JoinDate = new DateTime(2019, 4, 20),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 7,
                            Address = new Address
                            {
                                AddressLineOne = "15 Granite Road",
                                AddressLineTwo = "Suite 205",
                                City = "Minerstown",
                                StateProvince = "Ore County",
                                PostalCode = "G1R2A8",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 8,
                            MemberFirstName = "Jessica",
                            MemberMiddleName = "Lamay",
                            MemberLastName = "Martinez",
                            JoinDate = new DateTime(2023, 1, 1),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 8,
                            Address = new Address
                            {
                                AddressLineOne = "876 Redwood Drive",
                                AddressLineTwo = null,
                                City = "Steelville",
                                StateProvince = "Forge State",
                                PostalCode = "R4D6W2",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 9,
                            MemberFirstName = "James",
                            MemberMiddleName = "Con",
                            MemberLastName = "Lee",
                            JoinDate = new DateTime(2022, 6, 30),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 9,
                            Address = new Address
                            {
                                AddressLineOne = "654 Willow Avenue",
                                AddressLineTwo = "Floor 3",
                                City = "Ironworks",
                                StateProvince = "Steel Kingdom",
                                PostalCode = "W2L5A7",
                                Country = "Australia"
                            }
                        },
                        new Member
                        {
                            ID = 10,
                            MemberFirstName = "Olivia",
                            MemberMiddleName = "Rody",
                            MemberLastName = "Harris",
                            JoinDate = new DateTime(2020, 2, 25),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 10,
                            Address = new Address
                            {
                                AddressLineOne = "99 Ash Boulevard",
                                AddressLineTwo = "Building A",
                                City = "Metaltown",
                                StateProvince = "Steel Nation",
                                PostalCode = "A9H4L2",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 11,
                            MemberFirstName = "Liam",
                            MemberMiddleName = null,
                            MemberLastName = "Garcia",
                            JoinDate = new DateTime(2021, 7, 10),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 11,
                            Address = new Address
                            {
                                AddressLineOne = "321 Birch Parkway",
                                AddressLineTwo = null,
                                City = "Forgestone",
                                StateProvince = "Metal Hills",
                                PostalCode = "B3P7K9",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 12,
                            MemberFirstName = "Sophia",
                            MemberMiddleName = "Eeren",
                            MemberLastName = "Rodriguez",
                            JoinDate = new DateTime(2023, 3, 15),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 12,
                            Address = new Address
                            {
                                AddressLineOne = "789 Poplar Court",
                                AddressLineTwo = "Apt 12C",
                                City = "Alloyton",
                                StateProvince = "Forge Land",
                                PostalCode = "P1L3A8",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 13,
                            MemberFirstName = "Benjamin",
                            MemberMiddleName = null,
                            MemberLastName = "Clark",
                            JoinDate = new DateTime(2020, 8, 19),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 13,
                            Address = new Address
                            {
                                AddressLineOne = "102 Maple Circle",
                                AddressLineTwo = null,
                                City = "Copperlake",
                                StateProvince = "Metal Coast",
                                PostalCode = "M2C4L7",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 14,
                            MemberFirstName = "Emma",
                            MemberMiddleName = null,
                            MemberLastName = "Lopez",
                            JoinDate = new DateTime(2021, 5, 20),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 14,
                            Address = new Address
                            {
                                AddressLineOne = "455 Oak Crescent",
                                AddressLineTwo = "Suite 8",
                                City = "Iron Bay",
                                StateProvince = "Steel Region",
                                PostalCode = "O5C8L4",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 15,
                            MemberFirstName = "Alexander",
                            MemberMiddleName = null,
                            MemberLastName = "Walker",
                            JoinDate = new DateTime(2022, 11, 18),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 15,
                            Address = new Address
                            {
                                AddressLineOne = "678 Elmwood Drive",
                                AddressLineTwo = null,
                                City = "Iron Harbor",
                                StateProvince = "Metal Territory",
                                PostalCode = "E5L8M2",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 16,
                            MemberFirstName = "Isabella",
                            MemberMiddleName = "Maria",
                            MemberLastName = "Young",
                            JoinDate = new DateTime(2023, 4, 5),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 16,
                            Address = new Address
                            {
                                AddressLineOne = "150 Pine View Lane",
                                AddressLineTwo = "Unit 11A",
                                City = "Steel Ridge",
                                StateProvince = "Forge County",
                                PostalCode = "P2V3L1",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 17,
                            MemberFirstName = "Ethan",
                            MemberMiddleName = "Jordan",
                            MemberLastName = "Hall",
                            JoinDate = new DateTime(2019, 9, 30),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 17,
                            Address = new Address
                            {
                                AddressLineOne = "982 Cedar Grove",
                                AddressLineTwo = null,
                                City = "Alloyville",
                                StateProvince = "Iron District",
                                PostalCode = "C9D8V6",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 18,
                            MemberFirstName = "Mia",
                            MemberMiddleName = null,
                            MemberLastName = "Adams",
                            JoinDate = new DateTime(2020, 7, 15),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 18,
                            Address = new Address
                            {
                                AddressLineOne = "455 Redwood Trail",
                                AddressLineTwo = "Building 2",
                                City = "Forge City",
                                StateProvince = "Steel Plateau",
                                PostalCode = "R1D6C8",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 19,
                            MemberFirstName = "Daniel",
                            MemberMiddleName = "Amber",
                            MemberLastName = "Hernandez",
                            JoinDate = new DateTime(2021, 10, 10),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 19,
                            Address = new Address
                            {
                                AddressLineOne = "210 Birchwood Terrace",
                                AddressLineTwo = null,
                                City = "Copperton",
                                StateProvince = "Forge Hills",
                                PostalCode = "B7L9P3",
                                Country = "Australia"
                            }
                        },
                        new Member
                        {
                            ID = 20,
                            MemberFirstName = "Ava",
                            MemberMiddleName = null,
                            MemberLastName = "Roberts",
                            JoinDate = new DateTime(2022, 12, 25),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 20,
                            Address = new Address
                            {
                                AddressLineOne = "300 Maple Valley",
                                AddressLineTwo = "Suite 7B",
                                City = "Iron Bridge",
                                StateProvince = "Metal County",
                                PostalCode = "M8C9B5",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 21,
                            MemberFirstName = "Lucas",
                            MemberMiddleName = "Rody",
                            MemberLastName = "Perez",
                            JoinDate = new DateTime(2018, 3, 3),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 21,
                            Address = new Address
                            {
                                AddressLineOne = "150 Granite Street",
                                AddressLineTwo = "Floor 1",
                                City = "Steel Heights",
                                StateProvince = "Iron Plains",
                                PostalCode = "G1N5T8",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 22,
                            MemberFirstName = "Charlotte",
                            MemberMiddleName = "L.",
                            MemberLastName = "King",
                            JoinDate = new DateTime(2023, 8, 20),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 22,
                            Address = new Address
                            {
                                AddressLineOne = "489 Walnut Road",
                                AddressLineTwo = null,
                                City = "Forgeland",
                                StateProvince = "Metal Shores",
                                PostalCode = "W5R9K2",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 23,
                            MemberFirstName = "Henry",
                            MemberMiddleName = null,
                            MemberLastName = "Scott",
                            JoinDate = new DateTime(2020, 11, 8),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 23,
                            Address = new Address
                            {
                                AddressLineOne = "670 Cedar Hills",
                                AddressLineTwo = "Building 5",
                                City = "Ironwood",
                                StateProvince = "Steel Peninsula",
                                PostalCode = "I2R7L3",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 24,
                            MemberFirstName = "Amelia",
                            MemberMiddleName = "New",
                            MemberLastName = "Green",
                            JoinDate = new DateTime(2021, 2, 2),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 24,
                            Address = new Address
                            {
                                AddressLineOne = "90 Ashwood Lane",
                                AddressLineTwo = "Unit 6A",
                                City = "Copper Ridge",
                                StateProvince = "Metal Valley",
                                PostalCode = "A1L3T7",
                                Country = "Australia"
                            }
                        },
                        new Member
                        {
                            ID = 25,
                            MemberFirstName = "Jack",
                            MemberMiddleName = null,
                            MemberLastName = "White",
                            JoinDate = new DateTime(2019, 12, 15),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 25,
                            Address = new Address
                            {
                                AddressLineOne = "300 Maple Hill",
                                AddressLineTwo = null,
                                City = "Steelport",
                                StateProvince = "Iron Range",
                                PostalCode = "M4R2N1",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 26,
                            MemberFirstName = "Olivia",
                            MemberMiddleName = "Eron",
                            MemberLastName = "Taylor",
                            JoinDate = new DateTime(2020, 4, 18),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 26,
                            Address = new Address
                            {
                                AddressLineOne = "500 Birchwood Avenue",
                                AddressLineTwo = null,
                                City = "Forgedale",
                                StateProvince = "Metal Coast",
                                PostalCode = "F6G8H4",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 27,
                            MemberFirstName = "Noah",
                            MemberMiddleName = "Hood",
                            MemberLastName = "Martinez",
                            JoinDate = new DateTime(2023, 7, 25),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 27,
                            Address = new Address
                            {
                                AddressLineOne = "789 Ironwood Lane",
                                AddressLineTwo = "Suite 3B",
                                City = "Steelton",
                                StateProvince = "Forge Province",
                                PostalCode = "S3T5R2",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 28,
                            MemberFirstName = "Sophia",
                            MemberMiddleName = null,
                            MemberLastName = "Garcia",
                            JoinDate = new DateTime(2021, 5, 12),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 28,
                            Address = new Address
                            {
                                AddressLineOne = "213 Maplewood Drive",
                                AddressLineTwo = "Apt 14",
                                City = "Ironville",
                                StateProvince = "Steel Heights",
                                PostalCode = "I5R6T8",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 29,
                            MemberFirstName = "Mason",
                            MemberMiddleName = "Kotlyn",
                            MemberLastName = "Thomas",
                            JoinDate = new DateTime(2018, 9, 1),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 29,
                            Address = new Address
                            {
                                AddressLineOne = "112 Copper Valley Road",
                                AddressLineTwo = null,
                                City = "Alloytown",
                                StateProvince = "Metal District",
                                PostalCode = "C8D2R4",
                                Country = "Australia"
                            }
                        },
                        new Member
                        {
                            ID = 30,
                            MemberFirstName = "Ella",
                            MemberMiddleName = null,
                            MemberLastName = "Harris",
                            JoinDate = new DateTime(2022, 3, 17),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 30,
                            Address = new Address
                            {
                                AddressLineOne = "75 Elmwood Circle",
                                AddressLineTwo = "Floor 2",
                                City = "Forgeland",
                                StateProvince = "Iron Region",
                                PostalCode = "E7F8G9",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 31,
                            MemberFirstName = "James",
                            MemberMiddleName = "Lopez",
                            MemberLastName = "Nelson",
                            JoinDate = new DateTime(2020, 12, 22),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 31,
                            Address = new Address
                            {
                                AddressLineOne = "680 Pine Crest Road",
                                AddressLineTwo = null,
                                City = "Steel Ridge",
                                StateProvince = "Metal Canyon",
                                PostalCode = "P6R9T3",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 32,
                            MemberFirstName = "Grace",
                            MemberMiddleName = "Martinez",
                            MemberLastName = "Clark",
                            JoinDate = new DateTime(2019, 7, 10),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 32,
                            Address = new Address
                            {
                                AddressLineOne = "145 Ashwood Lane",
                                AddressLineTwo = "Unit 9",
                                City = "Iron Harbor",
                                StateProvince = "Forge Peninsula",
                                PostalCode = "A2L5C8",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 33,
                            MemberFirstName = "Benjamin",
                            MemberMiddleName = null,
                            MemberLastName = "Lewis",
                            JoinDate = new DateTime(2021, 10, 14),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 33,
                            Address = new Address
                            {
                                AddressLineOne = "390 Cedar Park Drive",
                                AddressLineTwo = null,
                                City = "Copperton",
                                StateProvince = "Metal Zone",
                                PostalCode = "C3T6R2",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 34,
                            MemberFirstName = "Emily",
                            MemberMiddleName = "Nethan",
                            MemberLastName = "Young",
                            JoinDate = new DateTime(2022, 5, 27),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 34,
                            Address = new Address
                            {
                                AddressLineOne = "870 Birch View Lane",
                                AddressLineTwo = "Suite 2A",
                                City = "Steelville",
                                StateProvince = "Iron Plateau",
                                PostalCode = "B8L2M5",
                                Country = "Australia"
                            }
                        },
                        new Member
                        {
                            ID = 35,
                            MemberFirstName = "Henry",
                            MemberMiddleName = "A.",
                            MemberLastName = "Walker",
                            JoinDate = new DateTime(2019, 4, 30),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 35,
                            Address = new Address
                            {
                                AddressLineOne = "540 Maple Street",
                                AddressLineTwo = null,
                                City = "Alloyport",
                                StateProvince = "Forge Territory",
                                PostalCode = "M5N3T9",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 36,
                            MemberFirstName = "Abigail",
                            MemberMiddleName = "Ember",
                            MemberLastName = "Hill",
                            JoinDate = new DateTime(2020, 6, 18),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 36,
                            Address = new Address
                            {
                                AddressLineOne = "235 Ironwood Blvd",
                                AddressLineTwo = "Unit 10",
                                City = "Forgeland",
                                StateProvince = "Metal Highlands",
                                PostalCode = "F4G8T2",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 37,
                            MemberFirstName = "Lucas",
                            MemberMiddleName = "Ralph",
                            MemberLastName = "Carter",
                            JoinDate = new DateTime(2021, 1, 19),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 37,
                            Address = new Address
                            {
                                AddressLineOne = "999 Oak Valley",
                                AddressLineTwo = "Building 8",
                                City = "Iron Heights",
                                StateProvince = "Steel Peninsula",
                                PostalCode = "I7R4M3",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 38,
                            MemberFirstName = "Chloe",
                            MemberMiddleName = "Lindsy",
                            MemberLastName = "Green",
                            JoinDate = new DateTime(2022, 9, 12),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 38,
                            Address = new Address
                            {
                                AddressLineOne = "455 Maple View",
                                AddressLineTwo = null,
                                City = "Steel Ridge",
                                StateProvince = "Forge Zone",
                                PostalCode = "M6R2L8",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 39,
                            MemberFirstName = "Mason",
                            MemberMiddleName = null,
                            MemberLastName = "Evans",
                            JoinDate = new DateTime(2018, 8, 10),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 39,
                            Address = new Address
                            {
                                AddressLineOne = "67 Elm Park Road",
                                AddressLineTwo = "Suite 1C",
                                City = "Iron Valley",
                                StateProvince = "Metal Canyon",
                                PostalCode = "E2R5L9",
                                Country = "Australia"
                            }
                        },
                        new Member
                        {
                            ID = 40,
                            MemberFirstName = "Amelia",
                            MemberMiddleName = "Kaper",
                            MemberLastName = "Perez",
                            JoinDate = new DateTime(2019, 3, 5),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 40,
                            Address = new Address
                            {
                                AddressLineOne = "310 Birch Grove",
                                AddressLineTwo = null,
                                City = "Alloytown",
                                StateProvince = "Steel Coast",
                                PostalCode = "A3N5M1",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 41,
                            MemberFirstName = "James",
                            MemberMiddleName = null,
                            MemberLastName = "Reed",
                            JoinDate = new DateTime(2020, 11, 25),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 41,
                            Address = new Address
                            {
                                AddressLineOne = "175 Pinewood Ave",
                                AddressLineTwo = "Floor 3",
                                City = "Copperport",
                                StateProvince = "Iron Ridge",
                                PostalCode = "P9R3L7",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 42,
                            MemberFirstName = "Olivia",
                            MemberMiddleName = "Marie",
                            MemberLastName = "Harris",
                            JoinDate = new DateTime(2022, 7, 14),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 42,
                            Address = new Address
                            {
                                AddressLineOne = "890 Maple Heights",
                                AddressLineTwo = "Unit 12",
                                City = "Steelton",
                                StateProvince = "Forge Region",
                                PostalCode = "S4T8M6",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 43,
                            MemberFirstName = "Liam",
                            MemberMiddleName = "Toot",
                            MemberLastName = "Campbell",
                            JoinDate = new DateTime(2021, 6, 21),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 43,
                            Address = new Address
                            {
                                AddressLineOne = "650 Ironwood Lane",
                                AddressLineTwo = "Suite 9B",
                                City = "Forgeland",
                                StateProvince = "Metal Territory",
                                PostalCode = "F8L2T3",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 44,
                            MemberFirstName = "Sophia",
                            MemberMiddleName = "Root",
                            MemberLastName = "King",
                            JoinDate = new DateTime(2019, 5, 30),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 44,
                            Address = new Address
                            {
                                AddressLineOne = "100 Elmview Road",
                                AddressLineTwo = "Apt 3D",
                                City = "Steel Ridge",
                                StateProvince = "Iron Zone",
                                PostalCode = "E9R4M2",
                                Country = "Australia"
                            }
                        },
                        new Member
                        {
                            ID = 45,
                            MemberFirstName = "Emma",
                            MemberMiddleName = "N.",
                            MemberLastName = "Lewis",
                            JoinDate = new DateTime(2023, 2, 17),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 45,
                            Address = new Address
                            {
                                AddressLineOne = "360 Maple Grove",
                                AddressLineTwo = null,
                                City = "Alloytown",
                                StateProvince = "Metal Shores",
                                PostalCode = "M2R5L8",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 46,
                            MemberFirstName = "Benjamin",
                            MemberMiddleName = null,
                            MemberLastName = "Moore",
                            JoinDate = new DateTime(2021, 9, 9),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 1,
                            Address = new Address
                            {
                                AddressLineOne = "550 Cedar Valley",
                                AddressLineTwo = "Suite 4A",
                                City = "Copperport",
                                StateProvince = "Iron Valley",
                                PostalCode = "C6N3M4",
                                Country = "USA"
                            }
                        },
                        new Member
                        {
                            ID = 47,
                            MemberFirstName = "Isabella",
                            MemberMiddleName = "F.",
                            MemberLastName = "Morgan",
                            JoinDate = new DateTime(2020, 4, 11),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 2,
                            Address = new Address
                            {
                                AddressLineOne = "770 Oakleaf Drive",
                                AddressLineTwo = null,
                                City = "Metal City",
                                StateProvince = "Iron Plains",
                                PostalCode = "O1M3T7",
                                Country = "Canada"
                            }
                        },
                        new Member
                        {
                            ID = 48,
                            MemberFirstName = "Alexander",
                            MemberMiddleName = "Jeremie",
                            MemberLastName = "Taylor",
                            JoinDate = new DateTime(2022, 10, 7),
                            StandingStatus = StandingStatus.Inactive,
                            OrganizationID = 3,
                            Address = new Address
                            {
                                AddressLineOne = "145 Steel Valley",
                                AddressLineTwo = "Building 5C",
                                City = "Forgeland",
                                StateProvince = "Metal Heights",
                                PostalCode = "S3V5L2",
                                Country = "UK"
                            }
                        },
                        new Member
                        {
                            ID = 49,
                            MemberFirstName = "Mia",
                            MemberMiddleName = "Han",
                            MemberLastName = "Baker",
                            JoinDate = new DateTime(2021, 3, 3),
                            StandingStatus = StandingStatus.Good,
                            OrganizationID = 4,
                            Address = new Address
                            {
                                AddressLineOne = "92 Maple Way",
                                AddressLineTwo = null,
                                City = "Steelton",
                                StateProvince = "Forge Valley",
                                PostalCode = "M7T2L9",
                                Country = "Australia"
                            }
                        },
                        new Member
                        {
                            ID = 50,
                            MemberFirstName = "Elijah",
                            MemberMiddleName = null,
                            MemberLastName = "Hughes",
                            JoinDate = new DateTime(2019, 12, 15),
                            StandingStatus = StandingStatus.Cancelled,
                            OrganizationID = 5,
                            Address = new Address
                            {
                                AddressLineOne = "333 Cedar Trail",
                                AddressLineTwo = "Apt 8F",
                                City = "Iron Heights",
                                StateProvince = "Metal District",
                                PostalCode = "C4N8M5",
                                Country = "USA"
                            }
                        }
                        );
                        context.SaveChanges();
                    }

                    if (!context.MemberMembershipTypes.Any())
                    {
                        context.MemberMembershipTypes.AddRange(
                            new MemberMembershipType
                            {
                                MemberID = 1, // John Doe
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 1, // John Doe
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 2, // Jane Smith
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 3, // Robert Johnson
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 4, // Emily Davis
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 5, // William Brown
                                MembershipTypeID = 2 // Silver Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 6, // Olivia Clark
                                MembershipTypeID = 5 // Platinum Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 7, // Noah Miller
                                MembershipTypeID = 3 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 8, // Sophia Wilson
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 9, // Liam Martinez
                                MembershipTypeID = 4 // Gold Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 10, // Ava Anderson
                                MembershipTypeID = 2 // Silver Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 11, // Mason Lee
                                MembershipTypeID = 5 //
                            },
                            new MemberMembershipType
                            {
                                MemberID = 12,
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 13,
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 14,
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 15,
                                MembershipTypeID = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 16,
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 17,
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 18,
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 19,
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 20,
                                MembershipTypeID = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 21,
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 22,
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 23,
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 24,
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 25,
                                MembershipTypeID = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 26,
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 27,
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 28,
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 29,
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 30,
                                MembershipTypeID = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 31,
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 32,
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 33,
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 34,
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 35,
                                MembershipTypeID = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 36,
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 37,
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 38,
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 39,
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 40,
                                MembershipTypeID = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 41,
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 42,
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 43,
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 44,
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 45,
                                MembershipTypeID = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 46,
                                MembershipTypeID = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 47,
                                MembershipTypeID = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 48,
                                MembershipTypeID = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 49,
                                MembershipTypeID = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberID = 50,
                                MembershipTypeID = 5 // Corporate Membership
                            }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Contacts.Any())
                    {
                        context.Contacts.AddRange(
                            new Contact
                            {
                                ID = 1,
                                ContactFirstName = "John",
                                ContactMiddleName = "Thomas",
                                ContactLastName = "Doe",
                                Title = "Manager",
                                Department = "Sales",
                                EMail = "john.doe@example.com",
                                Phone = "1234567890",
                                LinkedinUrl = "https://www.linkedin.com/in/johndoe",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 2,
                                ContactFirstName = "Jane",
                                ContactLastName = "Smith",
                                Title = "Director",
                                Department = "Marketing",
                                EMail = "jane.smith@example.com",
                                Phone = "9876543210",
                                LinkedinUrl = "https://www.linkedin.com/in/janesmith",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 3,
                                ContactFirstName = "Alice",
                                ContactLastName = "Johnson",
                                Title = "VP",
                                Department = "Human Resources",
                                EMail = "alice.johnson@example.com",
                                Phone = "5551234567",
                                LinkedinUrl = "https://www.linkedin.com/in/alicejohnson",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 4,
                                ContactFirstName = "Bob",
                                ContactMiddleName = "Joe",
                                ContactLastName = "Brown",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                EMail = "bob.brown@example.com",
                                Phone = "5557654321",
                                LinkedinUrl = "https://www.linkedin.com/in/bobbrown",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 5,
                                ContactFirstName = "Charlie",
                                ContactLastName = "Davis",
                                Title = "Chief Operating Officer",
                                Department = "Operations",
                                EMail = "charlie.davis@example.com",
                                Phone = "5557890123",
                                LinkedinUrl = "https://www.linkedin.com/in/charliedavis",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 6,
                                ContactFirstName = "Deborah",
                                ContactLastName = "Williams",
                                Title = "Director of Technology",
                                Department = "Technology",
                                EMail = "deborah.williams@example.com",
                                Phone = "5552345678",
                                LinkedinUrl = "https://www.linkedin.com/in/deborahwilliams",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 7,
                                ContactFirstName = "Eve",
                                ContactMiddleName = "Marie",
                                ContactLastName = "Taylor",
                                Title = "Marketing Specialist",
                                Department = "Marketing",
                                EMail = "eve.taylor@example.com",
                                Phone = "5553456789",
                                LinkedinUrl = "https://www.linkedin.com/in/evetaylor",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 8,
                                ContactFirstName = "Frank",
                                ContactLastName = "Harris",
                                Title = "Senior Engineer",
                                Department = "Engineering",
                                EMail = "frank.harris@example.com",
                                Phone = "5554567890",
                                LinkedinUrl = "https://www.linkedin.com/in/frankharris",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 9,
                                ContactFirstName = "Grace",
                                ContactLastName = "King",
                                Title = "Business Development Manager",
                                Department = "Sales",
                                EMail = "grace.king@example.com",
                                Phone = "5555678901",
                                LinkedinUrl = "https://www.linkedin.com/in/graceking",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 10,
                                ContactFirstName = "Hank",
                                ContactLastName = "Lee",
                                Title = "Head of Research",
                                Department = "Research and Development",
                                EMail = "hank.lee@example.com",
                                Phone = "5556789012",
                                LinkedinUrl = "https://www.linkedin.com/in/hanklee",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 11,
                                ContactFirstName = "Ivy",
                                ContactLastName = "Adams",
                                Title = "Project Manager",
                                Department = "Operations",
                                EMail = "ivy.adams@example.com",
                                Phone = "5557890123",
                                LinkedinUrl = "https://www.linkedin.com/in/ivyadams",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 12,
                                ContactFirstName = "Jack",
                                ContactLastName = "Scott",
                                Title = "CEO",
                                Department = "Executive",
                                EMail = "jack.scott@example.com",
                                Phone = "5558901234",
                                LinkedinUrl = "https://www.linkedin.com/in/jackscott",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 13,
                                ContactFirstName = "Kathy",
                                ContactMiddleName = "Elizabeth",
                                ContactLastName = "Morris",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                EMail = "kathy.morris@example.com",
                                Phone = "5559012345",
                                LinkedinUrl = "https://www.linkedin.com/in/kathymorris",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 14,
                                ContactFirstName = "Louis",
                                ContactMiddleName = "Alexandr",
                                ContactLastName = "Walker",
                                Title = "Customer Service Lead",
                                Department = "Customer Service",
                                EMail = "louis.walker@example.com",
                                Phone = "5550123456",
                                LinkedinUrl = "https://www.linkedin.com/in/louiswalker",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 15,
                                ContactFirstName = "Mona",
                                ContactMiddleName = "Grace",
                                ContactLastName = "White",
                                Title = "Legal Advisor",
                                Department = "Legal",
                                EMail = "mona.white@example.com",
                                Phone = "5551234567",
                                LinkedinUrl = "https://www.linkedin.com/in/monawhite",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 16,
                                ContactFirstName = "James",
                                ContactMiddleName = "T.",
                                ContactLastName = "Smith",
                                Title = "Marketing Manager",
                                Department = "Marketing",
                                EMail = "james.smith@example.com",
                                Phone = "5559876543",
                                LinkedinUrl = "https://www.linkedin.com/in/jamessmith",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 17,
                                ContactFirstName = "Sarah",
                                ContactMiddleName = "A.",
                                ContactLastName = "Johnson",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                EMail = "sarah.johnson@example.com",
                                Phone = "5551122334",
                                LinkedinUrl = "https://www.linkedin.com/in/sarahjohnson",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 18,
                                ContactFirstName = "David",
                                ContactMiddleName = "L.",
                                ContactLastName = "Brown",
                                Title = "Chief Executive Officer",
                                Department = "Executive",
                                EMail = "david.brown@example.com",
                                Phone = "5552233445",
                                LinkedinUrl = "https://www.linkedin.com/in/davidbrown",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 19,
                                ContactFirstName = "Emily",
                                ContactMiddleName = "M.",
                                ContactLastName = "Williams",
                                Title = "Product Designer",
                                Department = "Design",
                                EMail = "emily.williams@example.com",
                                Phone = "5556677889",
                                LinkedinUrl = "https://www.linkedin.com/in/emilywilliams",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 20,
                                ContactFirstName = "Michael",
                                ContactMiddleName = "J.",
                                ContactLastName = "Davis",
                                Title = "Sales Director",
                                Department = "Sales",
                                EMail = "michael.davis@example.com",
                                Phone = "5558899001",
                                LinkedinUrl = "https://www.linkedin.com/in/michaeldavis",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 21,
                                ContactFirstName = "Olivia",
                                ContactMiddleName = "K.",
                                ContactLastName = "Martinez",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                EMail = "olivia.martinez@example.com",
                                Phone = "5553456789",
                                LinkedinUrl = "https://www.linkedin.com/in/oliviamartinez",
                                IsVIP = true
                            },
                            new Contact
                            {
                                ID = 22,
                                ContactFirstName = "Ethan",
                                ContactMiddleName = "B.",
                                ContactLastName = "Taylor",
                                Title = "IT Manager",
                                Department = "IT",
                                EMail = "ethan.taylor@example.com",
                                Phone = "5552345678",
                                LinkedinUrl = "https://www.linkedin.com/in/ethantaylor",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 23,
                                ContactFirstName = "Sophia",
                                ContactMiddleName = "J.",
                                ContactLastName = "Wilson",
                                Title = "Operations Coordinator",
                                Department = "Operations",
                                EMail = "sophia.wilson@example.com",
                                Phone = "5556781234",
                                LinkedinUrl = "https://www.linkedin.com/in/sophiawilson",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 24,
                                ContactFirstName = "Daniel",
                                ContactMiddleName = "P.",
                                ContactLastName = "Moore",
                                Title = "Customer Success Manager",
                                Department = "Customer Support",
                                EMail = "daniel.moore@example.com",
                                Phone = "5559988776",
                                LinkedinUrl = "https://www.linkedin.com/in/danielmoore",
                                IsVIP = false
                            },
                            new Contact
                            {
                                ID = 25,
                                ContactFirstName = "Chloe",
                                ContactMiddleName = "S.",
                                ContactLastName = "Martin",
                                Title = "Senior Analyst",
                                Department = "Finance",
                                EMail = "chloe.martin@example.com",
                                Phone = "5557766554",
                                LinkedinUrl = "https://www.linkedin.com/in/chloemartin",
                                IsVIP = true
                            }
                        );
                        context.SaveChanges();
                    }

                    if (!context.ContactOrganizations.Any())
                    {
                        context.ContactOrganizations.AddRange(

                            new ContactOrganization { ContactID = 1, OrganizationID = 1 },
                            new ContactOrganization { ContactID = 1, OrganizationID = 2 },

                            new ContactOrganization { ContactID = 2, OrganizationID = 3 },
                            new ContactOrganization { ContactID = 2, OrganizationID = 4 },

                            new ContactOrganization { ContactID = 3, OrganizationID = 5 },
                            new ContactOrganization { ContactID = 3, OrganizationID = 6 },

                            new ContactOrganization { ContactID = 4, OrganizationID = 7 },
                            new ContactOrganization { ContactID = 4, OrganizationID = 8 },

                            new ContactOrganization { ContactID = 5, OrganizationID = 9 },
                            new ContactOrganization { ContactID = 5, OrganizationID = 10 },

                            new ContactOrganization { ContactID = 6, OrganizationID = 11 },
                            new ContactOrganization { ContactID = 6, OrganizationID = 12 },

                            new ContactOrganization { ContactID = 7, OrganizationID = 13 },
                            new ContactOrganization { ContactID = 7, OrganizationID = 14 },

                            new ContactOrganization { ContactID = 8, OrganizationID = 15 },
                            new ContactOrganization { ContactID = 8, OrganizationID = 16 },

                            new ContactOrganization { ContactID = 9, OrganizationID = 17 },
                            new ContactOrganization { ContactID = 9, OrganizationID = 18 },

                            new ContactOrganization { ContactID = 10, OrganizationID = 19 },
                            new ContactOrganization { ContactID = 10, OrganizationID = 20 },

                            new ContactOrganization { ContactID = 11, OrganizationID = 21 },
                            new ContactOrganization { ContactID = 11, OrganizationID = 22 },

                            new ContactOrganization { ContactID = 12, OrganizationID = 23 },
                            new ContactOrganization { ContactID = 12, OrganizationID = 24 },

                            new ContactOrganization { ContactID = 13, OrganizationID = 25 },
                            new ContactOrganization { ContactID = 13, OrganizationID = 26 },

                            new ContactOrganization { ContactID = 14, OrganizationID = 27 },
                            new ContactOrganization { ContactID = 14, OrganizationID = 28 },

                            new ContactOrganization { ContactID = 15, OrganizationID = 29 },
                            new ContactOrganization { ContactID = 15, OrganizationID = 30 },

                            new ContactOrganization { ContactID = 16, OrganizationID = 31 },
                            new ContactOrganization { ContactID = 16, OrganizationID = 32 },

                            new ContactOrganization { ContactID = 17, OrganizationID = 33 },
                            new ContactOrganization { ContactID = 17, OrganizationID = 34 },

                            new ContactOrganization { ContactID = 18, OrganizationID = 35 },
                            new ContactOrganization { ContactID = 18, OrganizationID = 36 },

                            new ContactOrganization { ContactID = 19, OrganizationID = 37 },
                            new ContactOrganization { ContactID = 19, OrganizationID = 38 },

                            new ContactOrganization { ContactID = 20, OrganizationID = 39 },
                            new ContactOrganization { ContactID = 20, OrganizationID = 40 },

                            new ContactOrganization { ContactID = 21, OrganizationID = 41 },
                            new ContactOrganization { ContactID = 21, OrganizationID = 42 },

                            new ContactOrganization { ContactID = 22, OrganizationID = 43 },
                            new ContactOrganization { ContactID = 22, OrganizationID = 44 },

                            new ContactOrganization { ContactID = 23, OrganizationID = 45 },
                            new ContactOrganization { ContactID = 23, OrganizationID = 1 },

                            new ContactOrganization { ContactID = 24, OrganizationID = 2 },
                            new ContactOrganization { ContactID = 24, OrganizationID = 3 },

                            new ContactOrganization { ContactID = 25, OrganizationID = 4 },
                            new ContactOrganization { ContactID = 25, OrganizationID = 5 }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Opportunities.Any())
                    {
                        context.Opportunities.AddRange(
                            new Opportunity
                            {
                                OpportunityName = "New Partnership with TechCo",
                                OpportunityDescr = "Potential collaboration with TechCo to offer joint solutions.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 1,  // Assuming Organization with ID 1 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 5),
                                //    InteractionNote = "Initial discussion on potential partnership."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Software Development for HealthCorp",
                                OpportunityDescr = "Software development project for HealthCorp to enhance their internal systems.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 2,  // Assuming Organization with ID 2 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 10),
                                //    InteractionNote = "Meeting to finalize project requirements."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Website Overhaul for FinServe",
                                OpportunityDescr = "Website redesign project for FinServe to improve their online presence.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 3,  // Assuming Organization with ID 3 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 15),
                                //    InteractionNote = "Final meeting to close project details."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Mobile App Development for EduTech",
                                OpportunityDescr = "Development of a mobile app for EduTech to expand their reach in the education sector.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 4,  // Assuming Organization with ID 4 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 18),
                                //    InteractionNote = "Kickoff meeting for mobile app project."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "CRM System for SalesForce",
                                OpportunityDescr = "Implementation of a CRM system for SalesForce to improve their customer relationship management.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 5,  // Assuming Organization with ID 5 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 20),
                                //    InteractionNote = "Meeting to discuss CRM system features."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "E-commerce Platform for ShopMart",
                                OpportunityDescr = "Development of a full-fledged e-commerce platform for ShopMart.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 6,  // Assuming Organization with ID 6 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 22),
                                //    InteractionNote = "Final review meeting before project closure."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "AI Integration for SmartTech",
                                OpportunityDescr = "Integration of AI-based solutions for SmartTech's systems.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 7,  // Assuming Organization with ID 7 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 25),
                                //    InteractionNote = "Discussion about AI integration and scope."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Cloud Migration for DataCloud",
                                OpportunityDescr = "Cloud migration for DataCloud to streamline their operations and storage.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 8,  // Assuming Organization with ID 8 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 28),
                                //    InteractionNote = "Discussing cloud architecture for migration."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Cybersecurity for SecureNet",
                                OpportunityDescr = "Cybersecurity services for SecureNet to enhance their data protection.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 9,  // Assuming Organization with ID 9 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 1, 30),
                                //    InteractionNote = "Final agreement on cybersecurity solutions."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Data Analytics for AnalyticsPro",
                                OpportunityDescr = "Implementing a data analytics platform for AnalyticsPro to improve decision-making.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 10,  // Assuming Organization with ID 10 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 2),
                                //    InteractionNote = "Initial discussion on data analytics requirements."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Business Intelligence for BizIntel",
                                OpportunityDescr = "Providing business intelligence solutions for BizIntel to enhance reporting capabilities.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 11,  // Assuming Organization with ID 11 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 5),
                                //    InteractionNote = "Meeting to review BI system features."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "ERP System for GlobalCorp",
                                OpportunityDescr = "ERP system implementation for GlobalCorp to streamline operations.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 12,  // Assuming Organization with ID 12 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 7),
                                //    InteractionNote = "Reviewing implementation plan and timeline."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Virtual Events Platform for EventPro",
                                OpportunityDescr = "Development of a platform for virtual events for EventPro.",
                                OpportunityStatus = OpportunityStatus.Open,
                                OrganizationID = 13,  // Assuming Organization with ID 13 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 10),
                                //    InteractionNote = "Kickoff meeting to discuss platform features."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Logistics Management System for MoveIt",
                                OpportunityDescr = "Logistics management software for MoveIt to optimize their operations.",
                                OpportunityStatus = OpportunityStatus.InProgress,
                                OrganizationID = 14,  // Assuming Organization with ID 14 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 12),
                                //    InteractionNote = "Discussing project scope and logistics software needs."
                                //}
                            },
                            new Opportunity
                            {
                                OpportunityName = "Blockchain Solutions for ChainTech",
                                OpportunityDescr = "Developing blockchain-based solutions for ChainTech's supply chain management.",
                                OpportunityStatus = OpportunityStatus.Closed,
                                OrganizationID = 15,  // Assuming Organization with ID 15 exists
                                //Interaction = new Interaction
                                //{
                                //    InteractionDate = new DateTime(2025, 2, 14),
                                //    InteractionNote = "Final meeting on blockchain integration."
                                //}
                            }
                        );
                        context.SaveChanges();
                    }

                    if (!context.Interactions.Any())
                    {
                        context.Interactions.AddRange(
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 1),
                                InteractionNote = "Initial contact for potential collaboration.",
                                ContactID = 1,
                                MemberID = 1,
                                OpportunityID = 1
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 3),
                                InteractionNote = "Follow-up email regarding partnership details.",
                                ContactID = 2,
                                MemberID = 2,
                                OpportunityID = 2
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 5),
                                InteractionNote = "Scheduled call to discuss project needs.",
                                ContactID = 3,
                                MemberID = 3,
                                OpportunityID = 3
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 10),
                                InteractionNote = "Met to discuss contract terms and conditions.",
                                ContactID = 4,
                                MemberID = 4,
                                OpportunityID = 4
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 12),
                                InteractionNote = "Client inquiry on pricing models.",
                                ContactID = 5,
                                MemberID = 1,
                                OpportunityID = 5
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 15),
                                InteractionNote = "Discussed solution packages for enterprise clients.",
                                ContactID = 6,
                                MemberID = 2,
                                OpportunityID = 6
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 18),
                                InteractionNote = "Following up on service proposal.",
                                ContactID = 7,
                                MemberID = 3,
                                OpportunityID = 7
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 20),
                                InteractionNote = "Finalizing service agreement terms.",
                                ContactID = 8,
                                MemberID = 4,
                                OpportunityID = 8
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 22),
                                InteractionNote = "Agreement on next steps and deliverables.",
                                ContactID = 9,
                                MemberID = 1,
                                OpportunityID = 9
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 25),
                                InteractionNote = "Reviewing deliverables for upcoming project.",
                                ContactID = 10,
                                MemberID = 2,
                                OpportunityID = 10
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 28),
                                InteractionNote = "Update on progress and timeline.",
                                ContactID = 11,
                                MemberID = 3,
                                OpportunityID = 11
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 1, 30),
                                InteractionNote = "Follow-up on final proposal details.",
                                ContactID = 12,
                                MemberID = 4,
                                OpportunityID = 12
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 2, 2),
                                InteractionNote = "Final meeting before project launch.",
                                ContactID = 13,
                                MemberID = 1,
                                OpportunityID = 13
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 2, 5),
                                InteractionNote = "Confirming details of service agreement.",
                                ContactID = 14,
                                MemberID = 2,
                                OpportunityID = 14
                            },
                            new Interaction
                            {
                                InteractionDate = new DateTime(2025, 2, 7),
                                InteractionNote = "Meeting to finalize documentation.",
                                ContactID = 3,
                                MemberID = 3,
                                OpportunityID = 15
                            }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Cancellations.Any())
                    {
                        context.Cancellations.AddRange(
                            // 2 Canceled records
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 5),
                                Canceled = true,
                                CancellationNote = "Member requested cancellation due to personal reasons.",
                                MemberID = 1  // Assuming Member with ID 1 exists
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 15),
                                Canceled = true,
                                CancellationNote = "Member canceled their subscription after failing to make payments.",
                                MemberID = 2  // Assuming Member with ID 2 exists
                            },

                            // 13 Non-Canceled records
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 10),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 3
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 12),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 4
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 18),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 5
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 20),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 6
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 22),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 7
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 25),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 8
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 27),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 9
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 30),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 10
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 1),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 11
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 5),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 12
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 10),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 13
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 15),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 14
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 18),
                                Canceled = false,
                                CancellationNote = "Active member, no cancellation.",
                                MemberID = 15
                            }
                        );
                        context.SaveChanges();

                        //Seed data needed for production and during development

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
