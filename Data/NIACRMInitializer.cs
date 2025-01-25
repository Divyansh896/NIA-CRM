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
                                WHERE rowId = NEW.rowId;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetProductionEmailTimestampOnInsert
                            AFTER INSERT ON ProductionEmail
                            BEGIN
                                UPDATE ProductionEmail
                                SET RowVersion = randomblob(8)
                                WHERE rowId = NEW.rowId;
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
                        new Industry { ID = 1, IndustryName = "Alpha Steel", IndustrySize = 250, WebsiteUrl = "https://www.alphasteel.com" },
                        new Industry { ID = 2, IndustryName = "TISCO CO.", IndustrySize = 150, WebsiteUrl = "https://www.tisco.com" },
                        new Industry { ID = 3, IndustryName = "M Time Irons", IndustrySize = 100, WebsiteUrl = "https://www.mtimeirons.com" },
                        new Industry { ID = 4, IndustryName = "Forge & Foundry Inc.", IndustrySize = 300, WebsiteUrl = "https://www.forgefoundry.com" },
                        new Industry { ID = 5, IndustryName = "Northern Metalworks", IndustrySize = 120, WebsiteUrl = "https://www.northernmetalworks.com" },
                        new Industry { ID = 6, IndustryName = "Titanium Solutions", IndustrySize = 400, WebsiteUrl = "https://www.titaniumsolutions.com" },
                        new Industry { ID = 7, IndustryName = "Phoenix Alloys", IndustrySize = 350, WebsiteUrl = "https://www.phoenixalloys.com" },
                        new Industry { ID = 8, IndustryName = "Galaxy Metals", IndustrySize = 500, WebsiteUrl = "https://www.galaxymetals.com" },
                        new Industry { ID = 9, IndustryName = "Ironclad Industries", IndustrySize = 220, WebsiteUrl = "https://www.ironcladindustries.com" },
                        new Industry { ID = 10, IndustryName = "Silverline Fabrication", IndustrySize = 180, WebsiteUrl = "https://www.silverlinefab.com" },
                        new Industry { ID = 11, IndustryName = "Star Steelworks", IndustrySize = 230, WebsiteUrl = "https://www.starsteelworks.com" },
                        new Industry { ID = 12, IndustryName = "Summit Metal Co.", IndustrySize = 270, WebsiteUrl = "https://www.summitmetalco.com" },
                        new Industry { ID = 13, IndustryName = "Everest Iron Corp.", IndustrySize = 210, WebsiteUrl = "https://www.everestironcorp.com" },
                        new Industry { ID = 14, IndustryName = "Prime Alloy Coatings", IndustrySize = 160, WebsiteUrl = "https://www.primealloycoatings.com" },
                        new Industry { ID = 15, IndustryName = "Magnum Steel Solutions", IndustrySize = 190, WebsiteUrl = "https://www.magnumsteel.com" },
                        new Industry { ID = 16, IndustryName = "Quantum Tech Innovations", IndustrySize = 450, WebsiteUrl = "https://www.quantumtechinnovations.com" },
                        new Industry { ID = 17, IndustryName = "Aurora Renewable Energy", IndustrySize = 500, WebsiteUrl = "https://www.aurorarenewable.com" },
                        new Industry { ID = 18, IndustryName = "Vertex Financial Group", IndustrySize = 80, WebsiteUrl = "https://www.vertexfinancialgroup.com" },
                        new Industry { ID = 19, IndustryName = "Nova Biotech Labs", IndustrySize = 60, WebsiteUrl = "https://www.novabiotechlabs.com" },
                        new Industry { ID = 20, IndustryName = "Summit Construction Co.", IndustrySize = 250, WebsiteUrl = "https://www.summitconstruction.com" },
                        new Industry { ID = 21, IndustryName = "Oceanic Shipping Corp", IndustrySize = 600, WebsiteUrl = "https://www.oceanicshipping.com" },
                        new Industry { ID = 22, IndustryName = "Evergreen Agriculture", IndustrySize = 550, WebsiteUrl = "https://www.evergreenagriculture.com" },
                        new Industry { ID = 23, IndustryName = "Ironclad Manufacturing Ltd.", IndustrySize = 300, WebsiteUrl = "https://www.ironcladmanufacturing.com" },
                        new Industry { ID = 24, IndustryName = "Skyline Architects Inc.", IndustrySize = 130, WebsiteUrl = "https://www.skylinearchitects.com" },
                        new Industry { ID = 25, IndustryName = "Pinnacle Consulting Services", IndustrySize = 90, WebsiteUrl = "https://www.pinnacleconsulting.com" },
                        new Industry { ID = 26, IndustryName = "Crystal Water Solutions", IndustrySize = 110, WebsiteUrl = "https://www.crystalwatersolutions.com" },
                        new Industry { ID = 27, IndustryName = "Elite Healthcare Partners", IndustrySize = 150, WebsiteUrl = "https://www.elitehealthcarepartners.com" },
                        new Industry { ID = 28, IndustryName = "Galaxy IT Solutions", IndustrySize = 400, WebsiteUrl = "https://www.galaxyitsolutions.com" },
                        new Industry { ID = 29, IndustryName = "Urban Infrastructure Group", IndustrySize = 350, WebsiteUrl = "https://www.urbaninfrastructure.com" },
                        new Industry { ID = 30, IndustryName = "Horizon Aerospace Inc.", IndustrySize = 450, WebsiteUrl = "https://www.horizonaerospace.com" },
                        new Industry { ID = 31, IndustryName = "Cobalt Mining Ventures", IndustrySize = 500, WebsiteUrl = "https://www.cobaltminingventures.com" },
                        new Industry { ID = 32, IndustryName = "LakesIde Resorts and Hotels", IndustrySize = 200, WebsiteUrl = "https://www.lakesideresorts.com" },
                        new Industry { ID = 33, IndustryName = "NextGen Media Productions", IndustrySize = 100, WebsiteUrl = "https://www.nextgenmediaproductions.com" },
                        new Industry { ID = 34, IndustryName = "Crestwood Pharmaceutical", IndustrySize = 120, WebsiteUrl = "https://www.crestwoodpharmaceutical.com" },
                        new Industry { ID = 35, IndustryName = "Dynamic Logistics Group", IndustrySize = 180, WebsiteUrl = "https://www.dynamiclogisticsgroup.com" },
                        new Industry { ID = 36, IndustryName = "Northern Timber Products", IndustrySize = 160, WebsiteUrl = "https://www.northerntimberproducts.com" },
                        new Industry { ID = 37, IndustryName = "Brightline Education Systems", IndustrySize = 50, WebsiteUrl = "https://www.brightlineeducationsystems.com" },
                        new Industry { ID = 38, IndustryName = "Fusion Energy Solutions", IndustrySize = 300, WebsiteUrl = "https://www.fusionenergysolutions.com" },
                        new Industry { ID = 39, IndustryName = "Trailblazer Automotive Group", IndustrySize = 450, WebsiteUrl = "https://www.trailblazerautomotive.com" },
                        new Industry { ID = 40, IndustryName = "Harvest Foods International", IndustrySize = 400, WebsiteUrl = "https://www.harvestfoods.com" },
                        new Industry { ID = 41, IndustryName = "Regal Entertainment Network", IndustrySize = 220, WebsiteUrl = "https://www.regalentertainmentnetwork.com" },
                        new Industry { ID = 42, IndustryName = "EcoSmart Waste Management", IndustrySize = 270, WebsiteUrl = "https://www.ecosmartwastemanagement.com" },
                        new Industry { ID = 43, IndustryName = "Summit Legal Services", IndustrySize = 130, WebsiteUrl = "https://www.summitlegalservices.com" },
                        new Industry { ID = 44, IndustryName = "Zenith Apparel Ltd.", IndustrySize = 80, WebsiteUrl = "https://www.zenithapparel.com" },
                        new Industry { ID = 45, IndustryName = "BlueWave Software Inc.", IndustrySize = 200, WebsiteUrl = "https://www.bluewavesoftware.com" }
                    );
                        context.SaveChanges();
                    }

                    if (!context.MembershipTypes.Any())
                    {
                        context.MembershipTypes.AddRange(
                            new MembershipType
                            {
                                ID = 1,
                                TypeName = "Associate",
                                TypeDescr = "Access to gym equipment and locker room facilities."
                            },
                            new MembershipType
                            {
                                ID = 2,
                                TypeName = "Chamber,Associate",
                                TypeDescr = "Includes Basic Membership benefits plus access to group classes and pool."
                            },
                            new MembershipType
                            {
                                ID = 3,
                                TypeName = "Government & Education,Associate",
                                TypeDescr = "Includes Premium Membership benefits for up to 4 family members."
                            },
                            new MembershipType
                            {
                                ID = 4,
                                TypeName = "Local Industrial",
                                TypeDescr = "Discounted membership for students with valId Id."
                            },
                            new MembershipType
                            {
                                ID = 5,
                                TypeName = "Non-Local Industrial",
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
                            Addresses = new List<Address>  // Correctly initialize Addresses as a collection
                            {
                                new Address
                                {
                                    AddressLine1 = "123 Elm St",
                                    AddressLine2 = "Apt 4B",
                                    City = "Metaltown",
                                    StateProvince = "Metal State",
                                    PostalCode = "M1E2L3",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 2,
                            MemberFirstName = "Jane",
                            MemberMiddleName = "Marie",
                            MemberLastName = "Smith",
                            JoinDate = new DateTime(2022, 1, 20),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "456 Oak Ave",
                                    AddressLine2 = null,
                                    City = "Ironville",
                                    StateProvince = "Steel Province",
                                    PostalCode = "I2R3O4",
                                    Country = "Canada"
                                }
                            }


                        },
                        new Member
                        {
                            ID = 3,
                            MemberFirstName = "Robert",
                            MemberMiddleName = "Joe",
                            MemberLastName = "Johnson",
                            JoinDate = new DateTime(2023, 3, 12),
                            StandingStatus = StandingStatus.Cancelled,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "789 Pine Rd",
                                    AddressLine2 = "Suite 302",
                                    City = "Steelton",
                                    StateProvince = "Metal Zone",
                                    PostalCode = "S3T4E5",
                                    Country = "UK"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 4,
                            MemberFirstName = "Emily",
                            MemberMiddleName = "Sharma",
                            MemberLastName = "Davis",
                            JoinDate = new DateTime(2022, 8, 8),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "789 Pine Rd",  // Corrected property name as per your requirement
                                    AddressLine2 = "Suite 302",   // Corrected property name as per your requirement
                                    City = "Steelton",
                                    StateProvince = "Metal Zone",
                                    PostalCode = "S3T4E5",
                                    Country = "UK"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 5,
                            MemberFirstName = "Michael",
                            MemberMiddleName = null,
                            MemberLastName = "Brown",
                            JoinDate = new DateTime(2020, 5, 10),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "201 Maple Street",  // Correct property name
                                    AddressLine2 = "Unit 101",          // Correct property name
                                    City = "Copperville",
                                    StateProvince = "Metalland",
                                    PostalCode = "C2P1E1",
                                    Country = "USA"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 6,
                            MemberFirstName = "Sarah",
                            MemberMiddleName = "Singh",
                            MemberLastName = "Johnson",
                            JoinDate = new DateTime(2021, 9, 15),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "543 Cedar Lane",  // Correct property name
                                    AddressLine2 = null,              // Correct property name
                                    City = "Ironcrest",
                                    StateProvince = "Steelstate",
                                    PostalCode = "I3N4O5",
                                    Country = "Canada"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 7,
                            MemberFirstName = "William",
                            MemberMiddleName = "Pat",
                            MemberLastName = "Taylor",
                            JoinDate = new DateTime(2019, 4, 20),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "15 Granite Road",  // Correct property name
                                    AddressLine2 = "Suite 205",       // Correct property name
                                    City = "Minerstown",
                                    StateProvince = "Ore County",
                                    PostalCode = "G1R2A8",
                                    Country = "UK"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 8,
                            MemberFirstName = "Jessica",
                            MemberMiddleName = "Divyansh",
                            MemberLastName = "Martinez",
                            JoinDate = new DateTime(2023, 1, 1),
                            StandingStatus = StandingStatus.Cancelled,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "876 Redwood Drive",  // Correct property name
                                    AddressLine2 = null,                 // Correct property name
                                    City = "Steelville",
                                    StateProvince = "Forge State",
                                    PostalCode = "R4D6W2",
                                    Country = "USA"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 9,
                            MemberFirstName = "James",
                            MemberMiddleName = "Hosi",
                            MemberLastName = "Lee",
                            JoinDate = new DateTime(2022, 6, 30),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "654 Willow Avenue",  // Correct property name
                                    AddressLine2 = "Floor 3",            // Correct property name
                                    City = "Ironworks",
                                    StateProvince = "Steel Kingdom",
                                    PostalCode = "W2L5A7",
                                    Country = "Australia"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 10,
                            MemberFirstName = "Olivia",
                            MemberMiddleName = null,
                            MemberLastName = "Rody",
                            JoinDate = new DateTime(2020, 2, 25),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>  // Initialize as a list
                            {
                                new Address
                                {
                                    AddressLine1 = "99 Ash Boulevard",
                                    AddressLine2 = "Building A",
                                    City = "Metaltown",
                                    StateProvince = "Steel Nation",
                                    PostalCode = "A9H4L2",
                                    Country = "Canada"
                                }
                            }

                        },
                        new Member
                        {
                            ID = 11,
                            MemberFirstName = "Liam",
                            MemberMiddleName = null,
                            MemberLastName = "Smith",
                            JoinDate = new DateTime(2021, 3, 17),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "12 Maple Street",
                                    AddressLine2 = "Suite 101",
                                    City = "Oakwood",
                                    StateProvince = "Green Valley",
                                    PostalCode = "G7F8M3",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 12,
                            MemberFirstName = "Emma",
                            MemberMiddleName = "Grace",
                            MemberLastName = "Johnson",
                            JoinDate = new DateTime(2022, 1, 5),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "45 Pine Road",
                                    AddressLine2 = "Apt 12B",
                                    City = "Springfield",
                                    StateProvince = "Illinois",
                                    PostalCode = "62704",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 13,
                            MemberFirstName = "Ava",
                            MemberMiddleName = "Marie",
                            MemberLastName = "Williams",
                            JoinDate = new DateTime(2019, 8, 22),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "99 Oak Drive",
                                    AddressLine2 = "Floor 3",
                                    City = "Rivertown",
                                    StateProvince = "Ocean State",
                                    PostalCode = "O4R2T1",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 14,
                            MemberFirstName = "Noah",
                            MemberMiddleName = "Alexander",
                            MemberLastName = "Brown",
                            JoinDate = new DateTime(2020, 6, 10),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "120 Birch Lane",
                                    AddressLine2 = "Unit 205",
                                    City = "Hillcrest",
                                    StateProvince = "Mountain Region",
                                    PostalCode = "M2N1K7",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 15,
                            MemberFirstName = "Sophia",
                            MemberMiddleName = null,
                            MemberLastName = "Davis",
                            JoinDate = new DateTime(2021, 4, 30),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "58 Cedar Street",
                                    AddressLine2 = "Building 4",
                                    City = "Brookside",
                                    StateProvince = "Blue Ridge",
                                    PostalCode = "B1A3N8",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 16,
                            MemberFirstName = "James",
                            MemberMiddleName = "Edward",
                            MemberLastName = "Martinez",
                            JoinDate = new DateTime(2018, 9, 15),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "111 Cherry Avenue",
                                    AddressLine2 = "Suite 500",
                                    City = "Silverbrook",
                                    StateProvince = "Sunshine State",
                                    PostalCode = "S2F9H1",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 17,
                            MemberFirstName = "Isabella",
                            MemberMiddleName = "Rose",
                            MemberLastName = "Taylor",
                            JoinDate = new DateTime(2021, 11, 9),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "78 Elm Street",
                                    AddressLine2 = "Floor 2",
                                    City = "Brighton",
                                    StateProvince = "New Horizons",
                                    PostalCode = "N5W2B4",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 18,
                            MemberFirstName = "Mason",
                            MemberMiddleName = "Joshua",
                            MemberLastName = "Anderson",
                            JoinDate = new DateTime(2022, 7, 21),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "21 Fir Street",
                                    AddressLine2 = "Unit 50",
                                    City = "New Haven",
                                    StateProvince = "Mystic Valley",
                                    PostalCode = "M3H8G2",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 19,
                            MemberFirstName = "Amelia",
                            MemberMiddleName = "Lynn",
                            MemberLastName = "Clark",
                            JoinDate = new DateTime(2021, 12, 18),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "36 Redwood Crescent",
                                    AddressLine2 = "Apartment 3",
                                    City = "Clearwater",
                                    StateProvince = "River Valley",
                                    PostalCode = "C8D2T5",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 20,
                            MemberFirstName = "Ethan",
                            MemberMiddleName = "Michael",
                            MemberLastName = "Garcia",
                            JoinDate = new DateTime(2020, 12, 14),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "64 Aspen Road",
                                    AddressLine2 = "Unit 207",
                                    City = "Forest Grove",
                                    StateProvince = "Woodland Hills",
                                    PostalCode = "W5T6D2",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 21,
                            MemberFirstName = "Charlotte",
                            MemberMiddleName = "Anne",
                            MemberLastName = "Hernandez",
                            JoinDate = new DateTime(2023, 2, 1),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "77 Riverbend Street",
                                    AddressLine2 = "Suite 9",
                                    City = "Silver Valley",
                                    StateProvince = "Sunset Coast",
                                    PostalCode = "S8K9J4",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 22,
                            MemberFirstName = "Benjamin",
                            MemberMiddleName = "David",
                            MemberLastName = "Moore",
                            JoinDate = new DateTime(2019, 5, 3),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "90 Willow Drive",
                                    AddressLine2 = "Floor 1",
                                    City = "Lakeside",
                                    StateProvince = "Silverwood",
                                    PostalCode = "L6C7S9",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 23,
                            MemberFirstName = "Ella",
                            MemberMiddleName = "Sophia",
                            MemberLastName = "Scott",
                            JoinDate = new DateTime(2020, 11, 19),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "102 Maple Lane",
                                    AddressLine2 = "Apt 1003",
                                    City = "Hill Valley",
                                    StateProvince = "Autumn Ridge",
                                    PostalCode = "A7X5Z2",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 24,
                            MemberFirstName = "Jack",
                            MemberMiddleName = null,
                            MemberLastName = "King",
                            JoinDate = new DateTime(2021, 8, 23),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "33 Ocean Breeze",
                                    AddressLine2 = "Unit 3",
                                    City = "Sunny Bay",
                                    StateProvince = "Tropical State",
                                    PostalCode = "T5Q1E2",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 25,
                            MemberFirstName = "Zoe",
                            MemberMiddleName = "Grace",
                            MemberLastName = "Lee",
                            JoinDate = new DateTime(2022, 9, 8),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "134 Rosewood Road",
                                    AddressLine2 = "Suite 101",
                                    City = "Lakeshore",
                                    StateProvince = "Northern Territory",
                                    PostalCode = "L8V2S3",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 26,
                            MemberFirstName = "Lucas",
                            MemberMiddleName = "Nathaniel",
                            MemberLastName = "Perez",
                            JoinDate = new DateTime(2021, 10, 5),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "56 Sunset Boulevard",
                                    AddressLine2 = "Building 2",
                                    City = "Greenridge",
                                    StateProvince = "Hill Valley",
                                    PostalCode = "G9T2R5",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 27,
                            MemberFirstName = "Mia",
                            MemberMiddleName = "Isabel",
                            MemberLastName = "Gonzalez",
                            JoinDate = new DateTime(2022, 2, 28),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "22 Brook Street",
                                    AddressLine2 = "Floor 5",
                                    City = "Sunrise City",
                                    StateProvince = "East Valley",
                                    PostalCode = "B3F6M2",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 28,
                            MemberFirstName = "Henry",
                            MemberMiddleName = "Charles",
                            MemberLastName = "Martinez",
                            JoinDate = new DateTime(2018, 11, 1),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "72 Birchwood Drive",
                                    AddressLine2 = "Apt 9B",
                                    City = "Riverwood",
                                    StateProvince = "Greenwood",
                                    PostalCode = "G1H8K3",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 29,
                            MemberFirstName = "Grace",
                            MemberMiddleName = "Lily",
                            MemberLastName = "Nguyen",
                            JoinDate = new DateTime(2022, 4, 19),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "23 Forest Avenue",
                                    AddressLine2 = "Unit 12",
                                    City = "Autumn Park",
                                    StateProvince = "Misty Hills",
                                    PostalCode = "F9D8P6",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 30,
                            MemberFirstName = "Daniel",
                            MemberMiddleName = "Jesse",
                            MemberLastName = "Harris",
                            JoinDate = new DateTime(2023, 1, 7),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "123 Oakwood Drive",
                                    AddressLine2 = "Apt 406",
                                    City = "Maple Town",
                                    StateProvince = "Riverdale",
                                    PostalCode = "M1H2N4",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 31,
                            MemberFirstName = "Sophie",
                            MemberMiddleName = "Claire",
                            MemberLastName = "Jackson",
                            JoinDate = new DateTime(2021, 5, 30),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "10 Willow Street",
                                    AddressLine2 = "Suite 10",
                                    City = "Starwood",
                                    StateProvince = "Golden Hills",
                                    PostalCode = "S1A7Q5",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 32,
                            MemberFirstName = "David",
                            MemberMiddleName = "Victor",
                            MemberLastName = "Roberts",
                            JoinDate = new DateTime(2019, 7, 23),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "58 Chestnut Lane",
                                    AddressLine2 = "Unit 4",
                                    City = "Pinehill",
                                    StateProvince = "Silverwood",
                                    PostalCode = "P7A6V9",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 33,
                            MemberFirstName = "Lily",
                            MemberMiddleName = null,
                            MemberLastName = "Adams",
                            JoinDate = new DateTime(2023, 5, 14),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "45 Sunset Way",
                                    AddressLine2 = "Building 6",
                                    City = "Crystal Falls",
                                    StateProvince = "Mountain Ridge",
                                    PostalCode = "C5H1N2",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 34,
                            MemberFirstName = "Jack",
                            MemberMiddleName = "Elliott",
                            MemberLastName = "Sanchez",
                            JoinDate = new DateTime(2021, 3, 12),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "89 Mountain Avenue",
                                    AddressLine2 = "Suite 301",
                                    City = "Rockport",
                                    StateProvince = "Redwood Hills",
                                    PostalCode = "R7P8X3",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 35,
                            MemberFirstName = "Natalie",
                            MemberMiddleName = null,
                            MemberLastName = "Mitchell",
                            JoinDate = new DateTime(2022, 6, 20),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "65 Pine Hill",
                                    AddressLine2 = "Apt 203",
                                    City = "Mapleton",
                                    StateProvince = "Sunny Fields",
                                    PostalCode = "M3L8W9",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 36,
                            MemberFirstName = "Ella",
                            MemberMiddleName = "Mae",
                            MemberLastName = "Kim",
                            JoinDate = new DateTime(2021, 6, 17),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "53 Bluebell Road",
                                    AddressLine2 = "Floor 4",
                                    City = "Riverside",
                                    StateProvince = "Green Hills",
                                    PostalCode = "B2F6T4",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 37,
                            MemberFirstName = "David",
                            MemberMiddleName = null,
                            MemberLastName = "Baker",
                            JoinDate = new DateTime(2019, 12, 5),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "31 Maple Terrace",
                                    AddressLine2 = "Unit 203",
                                    City = "Riverfield",
                                    StateProvince = "North Hills",
                                    PostalCode = "N8Y5D7",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 38,
                            MemberFirstName = "Charlotte",
                            MemberMiddleName = "Daisy",
                            MemberLastName = "Foster",
                            JoinDate = new DateTime(2020, 7, 1),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "77 Birchwood Crescent",
                                    AddressLine2 = "Apt 306",
                                    City = "Lakeview",
                                    StateProvince = "Horizon Bay",
                                    PostalCode = "L3D4S8",
                                    Country = "USA"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 39,
                            MemberFirstName = "Michael",
                            MemberMiddleName = "Harrison",
                            MemberLastName = "Evans",
                            JoinDate = new DateTime(2022, 3, 29),
                            StandingStatus = StandingStatus.Good,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "16 Redberry Lane",
                                    AddressLine2 = "Unit 4",
                                    City = "Silver Ridge",
                                    StateProvince = "Western Province",
                                    PostalCode = "S4G5A2",
                                    Country = "Canada"
                                }
                            }
                        },
                        new Member
                        {
                            ID = 40,
                            MemberFirstName = "Elena",
                            MemberMiddleName = "Paige",
                            MemberLastName = "Graham",
                            JoinDate = new DateTime(2021, 9, 25),
                            StandingStatus = StandingStatus.Inactive,
                            Addresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressLine1 = "19 Golden Court",
                                    AddressLine2 = "Suite 601",
                                    City = "Mountain Creek",
                                    StateProvince = "Silverstone",
                                    PostalCode = "G3J2B1",
                                    Country = "USA"
                                }
                            }
                        }

                        );
                        context.SaveChanges();
                    }
                    if (!context.MemberIndustries.Any())
                    {
                        context.MemberIndustries.AddRange(
                            new MemberIndustry { MemberId = 1, IndustryId = 1 },
                            new MemberIndustry { MemberId = 2, IndustryId = 2 },
                            new MemberIndustry { MemberId = 3, IndustryId = 3 },
                            new MemberIndustry { MemberId = 4, IndustryId = 4 },
                            new MemberIndustry { MemberId = 5, IndustryId = 5 },
                            new MemberIndustry { MemberId = 6, IndustryId = 6 },
                            new MemberIndustry { MemberId = 7, IndustryId = 7 },
                            new MemberIndustry { MemberId = 8, IndustryId = 8 },
                            new MemberIndustry { MemberId = 9, IndustryId = 9 },
                            new MemberIndustry { MemberId = 10, IndustryId = 10 },
                            new MemberIndustry { MemberId = 10, IndustryId = 41 },
                            new MemberIndustry { MemberId = 11, IndustryId = 11 },
                            new MemberIndustry { MemberId = 12, IndustryId = 12 },
                            new MemberIndustry { MemberId = 13, IndustryId = 13 },
                            new MemberIndustry { MemberId = 14, IndustryId = 14 },
                            new MemberIndustry { MemberId = 15, IndustryId = 15 },
                            new MemberIndustry { MemberId = 15, IndustryId = 42 },
                            new MemberIndustry { MemberId = 16, IndustryId = 16 },
                            new MemberIndustry { MemberId = 17, IndustryId = 17 },
                            new MemberIndustry { MemberId = 18, IndustryId = 18 },
                            new MemberIndustry { MemberId = 19, IndustryId = 19 },
                            new MemberIndustry { MemberId = 20, IndustryId = 20 },
                            new MemberIndustry { MemberId = 21, IndustryId = 21 },
                            new MemberIndustry { MemberId = 22, IndustryId = 22 },
                            new MemberIndustry { MemberId = 23, IndustryId = 23 },
                            new MemberIndustry { MemberId = 24, IndustryId = 24 },
                            new MemberIndustry { MemberId = 25, IndustryId = 25 },
                            new MemberIndustry { MemberId = 25, IndustryId = 43 },
                            new MemberIndustry { MemberId = 26, IndustryId = 26 },
                            new MemberIndustry { MemberId = 27, IndustryId = 27 },
                            new MemberIndustry { MemberId = 28, IndustryId = 28 },
                            new MemberIndustry { MemberId = 29, IndustryId = 29 },
                            new MemberIndustry { MemberId = 30, IndustryId = 30 },
                            new MemberIndustry { MemberId = 31, IndustryId = 31 },
                            new MemberIndustry { MemberId = 32, IndustryId = 32 },
                            new MemberIndustry { MemberId = 32, IndustryId = 44 },
                            new MemberIndustry { MemberId = 33, IndustryId = 33 },
                            new MemberIndustry { MemberId = 34, IndustryId = 34 },
                            new MemberIndustry { MemberId = 35, IndustryId = 35 },
                            new MemberIndustry { MemberId = 36, IndustryId = 36 },
                            new MemberIndustry { MemberId = 37, IndustryId = 37 },
                            new MemberIndustry { MemberId = 38, IndustryId = 38 },
                            new MemberIndustry { MemberId = 39, IndustryId = 39 },
                            new MemberIndustry { MemberId = 40, IndustryId = 40 },
                            new MemberIndustry { MemberId = 40, IndustryId = 45 }
                            );

                        context.SaveChanges();
                    }
                    if (!context.MemberMembershipTypes.Any())
                    {
                        context.MemberMembershipTypes.AddRange(
                            new MemberMembershipType
                            {
                                MemberId = 1, // John Doe
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 1, // John Doe
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 2, // Jane Smith
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 3, // Robert Johnson
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 4, // Emily Davis
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 5, // William Brown
                                MembershipTypeId = 2 // Silver Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 6, // Olivia Clark
                                MembershipTypeId = 5 // Platinum Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 7, // Noah Miller
                                MembershipTypeId = 3 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 8, // Sophia Wilson
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 9, // Liam Martinez
                                MembershipTypeId = 4 // Gold Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 10, // Ava Anderson
                                MembershipTypeId = 2 // Silver Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 11, // Mason Lee
                                MembershipTypeId = 5 //
                            },
                            new MemberMembershipType
                            {
                                MemberId = 12,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 13,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 14,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 15,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 16,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 17,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 18,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 19,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 20,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 21,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 22,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 23,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 24,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 25,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 26,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 27,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 28,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 29,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 30,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 31,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 32,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 33,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 34,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 35,
                                MembershipTypeId = 5 // Corporate Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 36,
                                MembershipTypeId = 1 // Basic Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 37,
                                MembershipTypeId = 4 // Student Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 38,
                                MembershipTypeId = 2 // Premium Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 39,
                                MembershipTypeId = 3 // Family Membership
                            },
                            new MemberMembershipType
                            {
                                MemberId = 40,
                                MembershipTypeId = 5 // Corporate Membership
                            }
                        );
                        context.SaveChanges();
                    }
                    if (!context.Contacts.Any())
                    {
                        context.Contacts.AddRange(
                            new Contact
                            {
                                Id = 1,
                                FirstName = "John",
                                LastName = "Thomas",
                                Title = "Manager",
                                Department = "Sales",
                                Email = "john.doe@example.com",
                                Phone = "1234567890",
                                LinkedInUrl = "https://www.linkedin.com/in/johndoe",
                                IsVip = true,
                                MemberId = 1
                            },
                            new Contact
                            {
                                Id = 2,
                                FirstName = "Jane",
                                LastName = "Smith",
                                Title = "Director",
                                Department = "Marketing",
                                Email = "jane.smith@example.com",
                                Phone = "9876543210",
                                LinkedInUrl = "https://www.linkedin.com/in/janesmith",
                                IsVip = false,
                                MemberId = 2
                            },
                            new Contact
                            {
                                Id = 3,
                                FirstName = "Alice",
                                LastName = "Johnson",
                                Title = "VP",
                                Department = "Human Resources",
                                Email = "alice.johnson@example.com",
                                Phone = "5551234567",
                                LinkedInUrl = "https://www.linkedin.com/in/alicejohnson",
                                IsVip = true,
                                MemberId = 3
                            },
                            new Contact
                            {
                                Id = 4,
                                FirstName = "Bob",
                                LastName = "Joe",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                Email = "bob.brown@example.com",
                                Phone = "5557654321",
                                LinkedInUrl = "https://www.linkedin.com/in/bobbrown",
                                IsVip = true,
                                MemberId = 4
                            },
                            new Contact
                            {
                                Id = 5,
                                FirstName = "Charlie",
                                LastName = "Davis",
                                Title = "Chief Operating Officer",
                                Department = "Operations",
                                Email = "charlie.davis@example.com",
                                Phone = "5557890123",
                                LinkedInUrl = "https://www.linkedin.com/in/charliedavis",
                                IsVip = false,
                                MemberId = 5
                            },
                            new Contact
                            {
                                Id = 6,
                                FirstName = "Deborah",
                                LastName = "Williams",
                                Title = "Director of Technology",
                                Department = "Technology",
                                Email = "deborah.williams@example.com",
                                Phone = "5552345678",
                                LinkedInUrl = "https://www.linkedin.com/in/deborahwilliams",
                                IsVip = true,
                                MemberId = 6
                            },
                            new Contact
                            {
                                Id = 7,
                                FirstName = "Eve",
                                LastName = "Marie",
                                Title = "Marketing Specialist",
                                Department = "Marketing",
                                Email = "eve.taylor@example.com",
                                Phone = "5553456789",
                                LinkedInUrl = "https://www.linkedin.com/in/evetaylor",
                                IsVip = false,
                                MemberId = 7
                            },
                            new Contact
                            {
                                Id = 8,
                                FirstName = "Frank",
                                LastName = "Harris",
                                Title = "Senior Engineer",
                                Department = "Engineering",
                                Email = "frank.harris@example.com",
                                Phone = "5554567890",
                                LinkedInUrl = "https://www.linkedin.com/in/frankharris",
                                IsVip = true,
                                MemberId = 8
                            },
                            new Contact
                            {
                                Id = 9,
                                FirstName = "Grace",
                                LastName = "King",
                                Title = "Business Development Manager",
                                Department = "Sales",
                                Email = "grace.king@example.com",
                                Phone = "5555678901",
                                LinkedInUrl = "https://www.linkedin.com/in/graceking",
                                IsVip = false,
                                MemberId = 9
                            },
                            new Contact
                            {
                                Id = 10,
                                FirstName = "Hank",
                                LastName = "Lee",
                                Title = "Head of Research",
                                Department = "Research and Development",
                                Email = "hank.lee@example.com",
                                Phone = "5556789012",
                                LinkedInUrl = "https://www.linkedin.com/in/hanklee",
                                IsVip = true,
                                MemberId = 10
                            },
                            new Contact
                            {
                                Id = 11,
                                FirstName = "Ivy",
                                LastName = "Adams",
                                Title = "Project Manager",
                                Department = "Operations",
                                Email = "ivy.adams@example.com",
                                Phone = "5557890123",
                                LinkedInUrl = "https://www.linkedin.com/in/ivyadams",
                                IsVip = false,
                                MemberId = 11
                            },
                            new Contact
                            {
                                Id = 12,
                                FirstName = "Jack",
                                LastName = "Scott",
                                Title = "CEO",
                                Department = "Executive",
                                Email = "jack.scott@example.com",
                                Phone = "5558901234",
                                LinkedInUrl = "https://www.linkedin.com/in/jackscott",
                                IsVip = true,
                                MemberId = 12
                            },
                            new Contact
                            {
                                Id = 13,
                                FirstName = "Kathy",
                                LastName = "Elizabeth",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                Email = "kathy.morris@example.com",
                                Phone = "5559012345",
                                LinkedInUrl = "https://www.linkedin.com/in/kathymorris",
                                IsVip = false,
                                MemberId = 13
                            },
                            new Contact
                            {
                                Id = 14,
                                FirstName = "Louis",
                                LastName = "Alexandr",
                                Title = "Customer Service Lead",
                                Department = "Customer Service",
                                Email = "louis.walker@example.com",
                                Phone = "5550123456",
                                LinkedInUrl = "https://www.linkedin.com/in/louiswalker",
                                IsVip = true,
                                MemberId = 14
                            },
                            new Contact
                            {
                                Id = 15,
                                FirstName = "Mona",
                                LastName = "Grace",
                                Title = "Legal Advisor",
                                Department = "Legal",
                                Email = "mona.white@example.com",
                                Phone = "5551234567",
                                LinkedInUrl = "https://www.linkedin.com/in/monawhite",
                                IsVip = false,
                                MemberId = 15
                            },
                            new Contact
                            {
                                Id = 16,
                                FirstName = "James",
                                LastName = "T.",
                                Title = "Marketing Manager",
                                Department = "Marketing",
                                Email = "james.smith@example.com",
                                Phone = "5559876543",
                                LinkedInUrl = "https://www.linkedin.com/in/jamessmith",
                                IsVip = false,
                                MemberId = 16
                            },
                            new Contact
                            {
                                Id = 17,
                                FirstName = "Sarah",
                                LastName = "A.",
                                Title = "HR Specialist",
                                Department = "Human Resources",
                                Email = "sarah.johnson@example.com",
                                Phone = "5551122334",
                                LinkedInUrl = "https://www.linkedin.com/in/sarahjohnson",
                                IsVip = false,
                                MemberId = 17
                            },
                            new Contact
                            {
                                Id = 18,
                                FirstName = "DavId",
                                LastName = "L.",
                                Title = "Chief Executive Officer",
                                Department = "Executive",
                                Email = "davId.brown@example.com",
                                Phone = "5552233445",
                                LinkedInUrl = "https://www.linkedin.com/in/davIdbrown",
                                IsVip = true,
                                MemberId = 18
                            },
                            new Contact
                            {
                                Id = 19,
                                FirstName = "Emily",
                                LastName = "M.",
                                Title = "Product Designer",
                                Department = "Design",
                                Email = "emily.williams@example.com",
                                Phone = "5556677889",
                                LinkedInUrl = "https://www.linkedin.com/in/emilywilliams",
                                IsVip = false,
                                MemberId = 19
                            },
                            new Contact
                            {
                                Id = 20,
                                FirstName = "Michael",
                                LastName = "J.",
                                Title = "Sales Director",
                                Department = "Sales",
                                Email = "michael.davis@example.com",
                                Phone = "5558899001",
                                LinkedInUrl = "https://www.linkedin.com/in/michaeldavis",
                                IsVip = false,
                                MemberId = 20
                            },
                            new Contact
                            {
                                Id = 21,
                                FirstName = "Olivia",
                                LastName = "K.",
                                Title = "Chief Financial Officer",
                                Department = "Finance",
                                Email = "olivia.martinez@example.com",
                                Phone = "5553456789",
                                LinkedInUrl = "https://www.linkedin.com/in/oliviamartinez",
                                IsVip = true,
                                MemberId = 21
                            },
                            new Contact
                            {
                                Id = 22,
                                FirstName = "Ethan",
                                LastName = "B.",
                                Title = "IT Manager",
                                Department = "IT",
                                Email = "ethan.taylor@example.com",
                                Phone = "5552345678",
                                LinkedInUrl = "https://www.linkedin.com/in/ethantaylor",
                                IsVip = false,
                                MemberId = 22
                            },
                            new Contact
                            {
                                Id = 23,
                                FirstName = "Sophia",
                                LastName = "J.",
                                Title = "Operations Coordinator",
                                Department = "Operations",
                                Email = "sophia.wilson@example.com",
                                Phone = "5556781234",
                                LinkedInUrl = "https://www.linkedin.com/in/sophiawilson",
                                IsVip = false,
                                MemberId = 23
                            },
                            new Contact
                            {
                                Id = 24,
                                FirstName = "Daniel",
                                LastName = "P.",
                                Title = "Customer Success Manager",
                                Department = "Customer Support",
                                Email = "daniel.moore@example.com",
                                Phone = "5559988776",
                                LinkedInUrl = "https://www.linkedin.com/in/danielmoore",
                                IsVip = false,
                                MemberId = 24
                            },
                        new Contact
                        {
                            Id = 25,
                            FirstName = "Chloe",
                            LastName = "S.",
                            Title = "Senior Analyst",
                            Department = "Finance",
                            Email = "chloe.martin@example.com",
                            Phone = "5557766554",
                            LinkedInUrl = "https://www.linkedin.com/in/chloemartin",
                            IsVip = true,
                            MemberId = 25
                        },
                        new Contact
                        {
                            Id = 26,
                            FirstName = "Liam",
                            LastName = "G.",
                            Title = "Project Manager",
                            Department = "Operations",
                            Email = "liam.green@example.com",
                            Phone = "5554433221",
                            LinkedInUrl = "https://www.linkedin.com/in/liamgreen",
                            IsVip = false,
                            MemberId = 26
                        },
                        new Contact
                        {
                            Id = 27,
                            FirstName = "Isabella",
                            LastName = "H.",
                            Title = "Marketing Director",
                            Department = "Marketing",
                            Email = "isabella.hudson@example.com",
                            Phone = "5559988776",
                            LinkedInUrl = "https://www.linkedin.com/in/isabellahudson",
                            IsVip = true,
                            MemberId = 27
                        },
                        new Contact
                        {
                            Id = 28,
                            FirstName = "Ethan",
                            LastName = "P.",
                            Title = "Sales Manager",
                            Department = "Sales",
                            Email = "ethan.peters@example.com",
                            Phone = "5551122334",
                            LinkedInUrl = "https://www.linkedin.com/in/ethanpeters",
                            IsVip = false,
                            MemberId = 28
                        },
                        new Contact
                        {
                            Id = 29,
                            FirstName = "Ava",
                            LastName = "W.",
                            Title = "Human Resources Specialist",
                            Department = "HR",
                            Email = "ava.williams@example.com",
                            Phone = "5556677889",
                            LinkedInUrl = "https://www.linkedin.com/in/avawilliams",
                            IsVip = true,
                            MemberId = 29
                        },
                        new Contact
                        {
                            Id = 30,
                            FirstName = "Mason",
                            LastName = "J.",
                            Title = "Data Scientist",
                            Department = "IT",
                            Email = "mason.james@example.com",
                            Phone = "5554455667",
                            LinkedInUrl = "https://www.linkedin.com/in/masonjames",
                            IsVip = false,
                            MemberId = 30
                        },
                        new Contact
                        {
                            Id = 31,
                            FirstName = "Sophia",
                            LastName = "K.",
                            Title = "Customer Support Lead",
                            Department = "Customer Service",
                            Email = "sophia.king@example.com",
                            Phone = "5552334455",
                            LinkedInUrl = "https://www.linkedin.com/in/sophiaking",
                            IsVip = true,
                            MemberId = 31
                        },
                        new Contact
                        {
                            Id = 32,
                            FirstName = "Jackson",
                            LastName = "T.",
                            Title = "Chief Technology Officer",
                            Department = "Technology",
                            Email = "jackson.taylor@example.com",
                            Phone = "5555555555",
                            LinkedInUrl = "https://www.linkedin.com/in/jacksontaylor",
                            IsVip = true,
                            MemberId = 32
                        },
                        new Contact
                        {
                            Id = 33,
                            FirstName = "Charlotte",
                            LastName = "L.",
                            Title = "Finance Manager",
                            Department = "Finance",
                            Email = "charlotte.larson@example.com",
                            Phone = "5552233446",
                            LinkedInUrl = "https://www.linkedin.com/in/charlottelarson",
                            IsVip = false,
                            MemberId = 33
                        },
                        new Contact
                        {
                            Id = 34,
                            FirstName = "Lucas",
                            LastName = "B.",
                            Title = "IT Specialist",
                            Department = "IT",
                            Email = "lucas.brown@example.com",
                            Phone = "5556677880",
                            LinkedInUrl = "https://www.linkedin.com/in/lucasbrown",
                            IsVip = true,
                            MemberId = 34
                        },
                        new Contact
                        {
                            Id = 35,
                            FirstName = "Mia",
                            LastName = "C.",
                            Title = "Legal Advisor",
                            Department = "Legal",
                            Email = "mia.carter@example.com",
                            Phone = "5553334446",
                            LinkedInUrl = "https://www.linkedin.com/in/miacarter",
                            IsVip = true,
                            MemberId = 35
                        },
                        new Contact
                        {
                            Id = 36,
                            FirstName = "Logan",
                            LastName = "D.",
                            Title = "Operations Manager",
                            Department = "Operations",
                            Email = "logan.davis@example.com",
                            Phone = "5553344556",
                            LinkedInUrl = "https://www.linkedin.com/in/logandavis",
                            IsVip = false,
                            MemberId = 36
                        },
                        new Contact
                        {
                            Id = 37,
                            FirstName = "Harper",
                            LastName = "M.",
                            Title = "Product Manager",
                            Department = "Product",
                            Email = "harper.morris@example.com",
                            Phone = "5555566778",
                            LinkedInUrl = "https://www.linkedin.com/in/harpermorris",
                            IsVip = true,
                            MemberId = 37
                        },
                        new Contact
                        {
                            Id = 38,
                            FirstName = "Benjamin",
                            LastName = "N.",
                            Title = "Chief Executive Officer",
                            Department = "Executive",
                            Email = "benjamin.nelson@example.com",
                            Phone = "5551223344",
                            LinkedInUrl = "https://www.linkedin.com/in/benjaminnelson",
                            IsVip = true,
                            MemberId = 38
                        },
                        new Contact
                        {
                            Id = 39,
                            FirstName = "Ella",
                            LastName = "O.",
                            Title = "Public Relations Manager",
                            Department = "PR",
                            Email = "ella.olson@example.com",
                            Phone = "5554455668",
                            LinkedInUrl = "https://www.linkedin.com/in/ellaolson",
                            IsVip = false,
                            MemberId = 39
                        },
                        new Contact
                        {
                            Id = 40,
                            FirstName = "James",
                            LastName = "P.",
                            Title = "Chief Marketing Officer",
                            Department = "Marketing",
                            Email = "james.phillips@example.com",
                            Phone = "5557768999",
                            LinkedInUrl = "https://www.linkedin.com/in/jamesphillips",
                            IsVip = true,
                            MemberId = 40
                        }

                        );
                        context.SaveChanges();
                    }

                    if (!context.ContactIndustries.Any())
                    {
                        context.ContactIndustries.AddRange(
                         new ContactIndustry { Id = 1, ContactId = 1, IndustryId = 1 },
                         new ContactIndustry { Id = 2, ContactId = 2, IndustryId = 2 },
                         new ContactIndustry { Id = 3, ContactId = 1, IndustryId = 3 },
                         new ContactIndustry { Id = 4, ContactId = 3, IndustryId = 1 },
                         new ContactIndustry { Id = 5, ContactId = 4, IndustryId = 2 },
                         new ContactIndustry { Id = 6, ContactId = 2, IndustryId = 3 },
                         new ContactIndustry { Id = 7, ContactId = 3, IndustryId = 4 },
                         new ContactIndustry { Id = 8, ContactId = 4, IndustryId = 5 },
                         new ContactIndustry { Id = 9, ContactId = 5, IndustryId = 1 },
                         new ContactIndustry { Id = 10, ContactId = 6, IndustryId = 2 },
                         new ContactIndustry { Id = 11, ContactId = 7, IndustryId = 3 },
                         new ContactIndustry { Id = 12, ContactId = 8, IndustryId = 4 },
                         new ContactIndustry { Id = 13, ContactId = 9, IndustryId = 5 },
                         new ContactIndustry { Id = 14, ContactId = 10, IndustryId = 1 },
                         new ContactIndustry { Id = 15, ContactId = 11, IndustryId = 2 },
                         new ContactIndustry { Id = 16, ContactId = 12, IndustryId = 3 },
                         new ContactIndustry { Id = 17, ContactId = 13, IndustryId = 4 },
                         new ContactIndustry { Id = 18, ContactId = 14, IndustryId = 5 },
                         new ContactIndustry { Id = 19, ContactId = 15, IndustryId = 1 },
                         new ContactIndustry { Id = 20, ContactId = 16, IndustryId = 2 },
                         new ContactIndustry { Id = 21, ContactId = 17, IndustryId = 3 },
                         new ContactIndustry { Id = 22, ContactId = 18, IndustryId = 4 },
                         new ContactIndustry { Id = 23, ContactId = 19, IndustryId = 5 },
                         new ContactIndustry { Id = 24, ContactId = 20, IndustryId = 1 },
                         new ContactIndustry { Id = 25, ContactId = 21, IndustryId = 2 },
                         new ContactIndustry { Id = 26, ContactId = 22, IndustryId = 3 },
                         new ContactIndustry { Id = 27, ContactId = 23, IndustryId = 4 },
                         new ContactIndustry { Id = 28, ContactId = 24, IndustryId = 5 },
                         new ContactIndustry { Id = 29, ContactId = 25, IndustryId = 1 },
                         new ContactIndustry { Id = 30, ContactId = 26, IndustryId = 2 },
                         new ContactIndustry { Id = 31, ContactId = 27, IndustryId = 3 },
                         new ContactIndustry { Id = 32, ContactId = 28, IndustryId = 4 },
                         new ContactIndustry { Id = 33, ContactId = 29, IndustryId = 5 },
                         new ContactIndustry { Id = 34, ContactId = 30, IndustryId = 6 },
                         new ContactIndustry { Id = 35, ContactId = 31, IndustryId = 7 },
                         new ContactIndustry { Id = 36, ContactId = 32, IndustryId = 8 },
                         new ContactIndustry { Id = 37, ContactId = 33, IndustryId = 9 },
                         new ContactIndustry { Id = 38, ContactId = 34, IndustryId = 10 },
                         new ContactIndustry { Id = 39, ContactId = 35, IndustryId = 11 },
                         new ContactIndustry { Id = 40, ContactId = 36, IndustryId = 12 }
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
                                MemberID = 1  // Assuming Member with Id 1 exists
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 15),
                                Canceled = true,
                                CancellationNote = "Member canceled their subscription after failing to make payments.",
                                MemberID = 2  // Assuming Member with Id 2 exists
                            },

                            // 13 Non-Canceled records
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 10),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 3
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 12),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 4
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 18),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 5
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 20),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 6
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 22),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 7
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 25),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 8
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 27),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 9
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 1, 30),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 10
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 1),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 11
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 5),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 12
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 10),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 13
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 15),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 14
                            },
                            new Cancellation
                            {
                                CancellationDate = new DateTime(2025, 2, 18),
                                Canceled = false,
                                CancellationNote = "Good member, no cancellation.",
                                MemberID = 15
                            }
                        );
                        context.SaveChanges();
                    }
                    //Seed data needed for production and during development

                    if (!context.ProductionEmails.Any())
                    {
                        // Random member names to be used in the Email content
                        var randomNames = new List<string>
                            {
                                "John Doe", "Jane Smith", "Robert Johnson", "Emily Davis", "Michael Brown"
                            };

                        // Seeding predefined production Email templates with random member names
                        context.ProductionEmails.AddRange(
                             new ProductionEmail
                             {
                                 Id = 1,
                                 EmailType = "Welcome Email",
                                 Subject = "Welcome to NIA!",
                                 Body = $"Dear {randomNames[0]},\n\nWelcome to the NIA community! We are thrilled to have you onboard. Please let us know if you need any assistance.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             },
                             new ProductionEmail
                             {
                                 Id = 2,
                                 EmailType = "Renewal Reminder",
                                 Subject = "Membership Renewal Reminder",
                                 Body = $"Dear {randomNames[1]},\n\nYour membership with NIA is about to expire on 2025-02-15. Please renew your membership to continue enjoying all the benefits.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             },
                             new ProductionEmail
                             {
                                 Id = 3,
                                 EmailType = "Cancellation Notice",
                                 Subject = "Membership Cancellation Confirmation",
                                 Body = $"Dear {randomNames[2]},\n\nWe are sorry to see you go. Your membership has been successfully canceled. If you change your mind in the future, we’d love to have you back.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             },
                             new ProductionEmail
                             {
                                 Id = 4,
                                 EmailType = "Membership Update",
                                 Subject = "Important Membership Update",
                                 Body = $"Dear {randomNames[3]},\n\nWe would like to inform you of an important update regarding your membership status. Please log in to your account for more details.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             },
                             new ProductionEmail
                             {
                                 Id = 5,
                                 EmailType = "General Notification",
                                 Subject = "NIA System Update",
                                 Body = $"Dear {randomNames[4]},\n\nThis is a notification regarding a recent update to the NIA system. We encourage you to check out the new features and improvements.\n\nBest regards,\nNIA Team",
                                 //IsGood = true
                             });
                        context.SaveChanges();
                    }

                    if (!context.MemberNotes.Any())
                    {
                        context.MemberNotes.AddRange(
                        new MemberNote { Id = 1, MemberId = 1, Note = "Initial onboarding discussion with the client about the new CRM implementation project.", CreatedAt = DateTime.Now.AddDays(-30) },
                        new MemberNote { Id = 2, MemberId = 2, Note = "Client follow-up meeting to discuss the status of the CRM project.", CreatedAt = DateTime.Now.AddDays(-28) },
                        new MemberNote { Id = 3, MemberId = 3, Note = "Reviewed the feedback provided by the client regarding the CRM customization.", CreatedAt = DateTime.Now.AddDays(-25) },
                        new MemberNote { Id = 4, MemberId = 4, Note = "Final meeting to present the completed CRM project and gather client feedback.", CreatedAt = DateTime.Now.AddDays(-20) },
                        new MemberNote { Id = 5, MemberId = 5, Note = "Discussed potential upgrades to the CRM system and next steps for implementation.", CreatedAt = DateTime.Now.AddDays(-15) },
                        new MemberNote { Id = 6, MemberId = 6, Note = "Follow-up meeting on CRM feedback and next implementation steps.", CreatedAt = DateTime.Now.AddDays(-14) },
                        new MemberNote { Id = 7, MemberId = 7, Note = "Presentation of CRM reports and how they impact client operations.", CreatedAt = DateTime.Now.AddDays(-13) },
                        new MemberNote { Id = 8, MemberId = 8, Note = "Discussed additional CRM modules with the client.", CreatedAt = DateTime.Now.AddDays(-12) },
                        new MemberNote { Id = 9, MemberId = 9, Note = "Client requested further training on CRM features.", CreatedAt = DateTime.Now.AddDays(-10) },
                        new MemberNote { Id = 10, MemberId = 10, Note = "Assessed CRM performance and discussed any issues with the client.", CreatedAt = DateTime.Now.AddDays(-9) },
                        new MemberNote { Id = 11, MemberId = 11, Note = "Discussed CRM data integration challenges with client.", CreatedAt = DateTime.Now.AddDays(-8) },
                        new MemberNote { Id = 12, MemberId = 12, Note = "Client praised CRM's data accuracy and reporting tools.", CreatedAt = DateTime.Now.AddDays(-7) },
                        new MemberNote { Id = 13, MemberId = 13, Note = "Discussed CRM security enhancements and data privacy concerns.", CreatedAt = DateTime.Now.AddDays(-6) },
                        new MemberNote { Id = 14, MemberId = 14, Note = "Review of CRM system uptime and performance metrics.", CreatedAt = DateTime.Now.AddDays(-5) },
                        new MemberNote { Id = 15, MemberId = 15, Note = "Final meeting to present CRM system's success and discuss potential upgrades.", CreatedAt = DateTime.Now.AddDays(-4) },
                        new MemberNote { Id = 16, MemberId = 16, Note = "Client follow-up on CRM project post-implementation.", CreatedAt = DateTime.Now.AddDays(-3) },
                        new MemberNote { Id = 17, MemberId = 17, Note = "CRM update meeting scheduled for next month.", CreatedAt = DateTime.Now.AddDays(-2) },
                        new MemberNote { Id = 18, MemberId = 18, Note = "Client expressed satisfaction with CRM customization.", CreatedAt = DateTime.Now.AddDays(-1) },
                        new MemberNote { Id = 19, MemberId = 19, Note = "Discussed the CRM's future roadmap with the client.", CreatedAt = DateTime.Now.AddDays(-30) },
                        new MemberNote { Id = 20, MemberId = 20, Note = "Explained the new CRM reporting features to the client.", CreatedAt = DateTime.Now.AddDays(-29) },
                        new MemberNote { Id = 21, MemberId = 21, Note = "Meeting to review CRM's feature requests and client requirements.", CreatedAt = DateTime.Now.AddDays(-28) },
                        new MemberNote { Id = 22, MemberId = 22, Note = "CRM performance review meeting with the client team.", CreatedAt = DateTime.Now.AddDays(-27) },
                        new MemberNote { Id = 23, MemberId = 23, Note = "Discussions on improving CRM speed and scalability.", CreatedAt = DateTime.Now.AddDays(-26) },
                        new MemberNote { Id = 24, MemberId = 24, Note = "Client feedback session on CRM user interface improvements.", CreatedAt = DateTime.Now.AddDays(-25) },
                        new MemberNote { Id = 25, MemberId = 25, Note = "Client to provide feedback on the newly integrated CRM modules.", CreatedAt = DateTime.Now.AddDays(-24) },
                        new MemberNote { Id = 26, MemberId = 26, Note = "Final training session on CRM features for the client team.", CreatedAt = DateTime.Now.AddDays(-23) },
                        new MemberNote { Id = 27, MemberId = 27, Note = "Discussed potential issues with CRM's third-party integration.", CreatedAt = DateTime.Now.AddDays(-22) },
                        new MemberNote { Id = 28, MemberId = 28, Note = "Client inquired about additional CRM customization options.", CreatedAt = DateTime.Now.AddDays(-21) },
                        new MemberNote { Id = 29, MemberId = 29, Note = "Debrief on CRM deployment and initial reactions from the client team.", CreatedAt = DateTime.Now.AddDays(-20) },
                        new MemberNote { Id = 30, MemberId = 30, Note = "Scheduled CRM review call for next week with the client.", CreatedAt = DateTime.Now.AddDays(-19) },
                        new MemberNote { Id = 31, MemberId = 31, Note = "Client requested demo on advanced CRM reporting capabilities.", CreatedAt = DateTime.Now.AddDays(-18) },
                        new MemberNote { Id = 32, MemberId = 32, Note = "Client shared feedback on CRM's mobile interface and app.", CreatedAt = DateTime.Now.AddDays(-17) },
                        new MemberNote { Id = 33, MemberId = 33, Note = "Discussed client satisfaction and future CRM upgrades.", CreatedAt = DateTime.Now.AddDays(-16) },
                        new MemberNote { Id = 34, MemberId = 34, Note = "Followed up with the client regarding CRM bug fixes.", CreatedAt = DateTime.Now.AddDays(-15) },
                        new MemberNote { Id = 35, MemberId = 35, Note = "Revisited CRM's customization options for the client.", CreatedAt = DateTime.Now.AddDays(-14) },
                        new MemberNote { Id = 36, MemberId = 36, Note = "Review meeting with the client about CRM's current status.", CreatedAt = DateTime.Now.AddDays(-13) },
                        new MemberNote { Id = 37, MemberId = 37, Note = "Client requested documentation for CRM integration with existing systems.", CreatedAt = DateTime.Now.AddDays(-12) },
                        new MemberNote { Id = 38, MemberId = 38, Note = "Explained CRM's data storage architecture to the client.", CreatedAt = DateTime.Now.AddDays(-11) },
                        new MemberNote { Id = 39, MemberId = 39, Note = "Client follow-up regarding CRM's email marketing tools.", CreatedAt = DateTime.Now.AddDays(-10) },
                        new MemberNote { Id = 40, MemberId = 40, Note = "Discussed CRM's scalability and future-proofing with the client.", CreatedAt = DateTime.Now.AddDays(-9) }
                        );

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
