using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using DSharpPlus.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.CommandsNext.Exceptions;

namespace HayaseBot.commands
{
    public class GameCommands : BaseCommandModule
    {
        // This is the sql data client ID
        string sqlClientACC = "Data Source=HAYASEYUUKA\\SQLEXPRESS;Initial Catalog=DiscordStorage;Integrated Security=True";

        private Random random = new Random();

        /*
         * EASIEST OF THEM ALL THE BEG COMMAND
         */

        [Command("beg")]
        [Cooldown(1, 12, CooldownBucketType.User)]
        public async Task BegCommand(CommandContext ctx)
        {
            string[] listOfBegged = new string[]
            {
                "You begged to an old lady give you: ",
                "You begged to Eddie Sheerun gave you: ",
                "You begged to Alone Musk and gave you: ",
                "You begged to MR.Meast and got: "
            };

            string[] listOfBeggedfailed = new string[]
            {
                "Ew! beggar! Go find a job!",
                "Filthy human being go beg to another person!",
                "Imagine begging for money!",
                "Sorry i don't have any money!"
            };

            int randomBegged = random.Next(0, listOfBegged.Length);
            string beggSel = listOfBegged[randomBegged];

            int randomBegged2 = random.Next(0, listOfBeggedfailed.Length);
            string beggSel2 = listOfBeggedfailed[randomBegged2];

            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var cmdUser = ctx.User.Id;
            int coinRandom = random.Next(0, 120);
            var endZero = coinRandom%10;

            // SQL CONNECT
            SqlConnection connection1 = new SqlConnection(sqlClientACC);
            connection1.Open();

            // Select the user from database
            // If not in the database then add first before adding the value of money
            string userSelectDB = "Select * From UserBank Where UserID = '"+cmdUser+"'";
            SqlCommand selectUser = new SqlCommand(userSelectDB, connection1);
            SqlDataReader readUser = selectUser.ExecuteReader();

            if (readUser.Read())
            {
                // Read the user Accon First
                SqlConnection connection2 = new SqlConnection(sqlClientACC);
                connection2.Open();
                string runCMDD = "Select Wallet From UserBank Where UserID = '"+cmdUser+"'";
                SqlCommand AddAcc = new SqlCommand(runCMDD, connection2);
                SqlDataReader readAcc = AddAcc.ExecuteReader();

                // Reads the user's account in the Database
                while (readAcc.Read())
                {
                    SqlConnection connection6 = new SqlConnection(sqlClientACC);
                    connection6.Open();
                    long currentBal = (long)readAcc["Wallet"];
                    long addedBal = currentBal + coinRandom;

                    // 2 separate command to add or deduct in the user's account
                    string runCommd = "UPDATE UserBank Set Wallet = '"+addedBal+"' Where UserID = '"+cmdUser+"'";

                    // IF THE NUMBER ENDS WITH ZERO OR IS ZERO
                    if (endZero == 0)
                    {
                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = beggSel2,
                            Color = randomCol,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = DateTime.Now.ToString("hh:mm tt"),
                                IconUrl = null
                            }
                        };
                        await ctx.RespondAsync(embed1);
                        return;
                    }
                    // Give the user the money
                    else
                    {
                        SqlCommand commnd = new SqlCommand(runCommd, connection6);
                        commnd.ExecuteNonQuery();

                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = beggSel + coinRandom + " Coins",
                            Color = randomCol,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = DateTime.Now.ToString("hh:mm tt"),
                                IconUrl = null
                            }
                        };
                        await ctx.RespondAsync(embed1);
                        connection6.Close();
                        return;
                    }
                }
            }
            else
            {
                /*
                 * THIS WILL REGISTER THE USER TO THE DATABASE
                 */

                // Set the user to the Database
                int defaultBank = 500;
                int defaultWallet = 500;
                SqlConnection connection3 = new SqlConnection(sqlClientACC);
                connection3.Open();

                // Add the user to the database with free 1000 coins
                string EditUser = "INSERT INTO UserBank(UserID,Bank,Wallet)values('"+cmdUser+"','"+defaultBank+"','"+defaultWallet+"')";
                SqlCommand runCMD = new SqlCommand(EditUser, connection3);
                runCMD.ExecuteNonQuery();
                connection3.Close();


                // Get the user's Current balance
                SqlConnection connection4 = new SqlConnection(sqlClientACC);
                connection4.Open();
                string getUserBalance = "Select Bank, Wallet From UserBank Where UserID = '"+cmdUser+"'";
                SqlCommand runCMD2 = new SqlCommand(getUserBalance, connection4);
                SqlDataReader readUserBalance = runCMD2.ExecuteReader();

                // Message the user if they are in the bank
                while (readUserBalance.Read())
                {
                    long walletBal = (long)readUserBalance["Wallet"];
                    long bankBal = (long)readUserBalance["Bank"];

                    // Message the user if they are in the bank which shows their balance
                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "Your accout has been registered to the System!",
                        Description = "Bank: " + bankBal + "\nWallet: " + walletBal,
                        Color = randomCol,
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            Text = DateTime.Now.ToString("hh:mm tt"),
                            IconUrl = null
                        }
                    };
                    await ctx.RespondAsync(embed1);
                }
                connection4.Close();
                connection1.Close();
                return;
            }
        }

        /*
         * THIS IS THE SEARCH SECTION; YEAH SO MANY RANDOMIZERS :D
         */

        [Command("search")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task SearchFor(CommandContext ctx, [RemainingText] string place = null)
        {
            try
            {
                // Color Randomizer
                int red = random.Next(256);
                int green = random.Next(256);
                int blue = random.Next(256);
                DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

                var cmdUser = ctx.User.Id;
                int coinRandom = random.Next(30, 251);

                // Array Of Strings to randomize messages for the user
                // Bed String Array
                string[] listOfBed = new string[]
                {
                    "You searched under the bed and found: ",
                    "You searched near the bed and found: ",
                    "You searched on your mom's bed and found: ",
                    "You searched under your mom's bed and found: ",
                    "You searched the boxes under the bed and found: ",
                    "You searched the old boxes under the bed and found: ",
                    "You searched under your bed and found: ",
                    "You searched the toy box near your bed and found: "
                };

                // Vault String Array
                string[] listOfVault = new string[]
                {
                    "You unlocked your vault and found: ",
                    "You unlocked your hidden vault and found: ",
                    "You lockpicked your neighbor's vault and found: ",
                    "You hammer a old vault and found: "
                };
                string[] listOfVaultfail = new string[]
                {
                    "You tried to lockpick your dad's vault but got Caught!",
                    "You tried to steal in your neighbor's vault and got Caught!",
                    "You entered the bank's vault to steal bot got Caught!",
                };

                // Pocket String Array
                string[] listOfPocket = new string[]
                {
                    "You unlocked your vault and found: ",
                    "You unlocked your hidden vault and found: ",
                    "You lockpicked your neighbor's vault and found: ",
                    "You hammer a old vault and found: "
                };
                string[] listOfPocketfail = new string[]
                {
                    "You tried to lockpick your dad's vault but got Caught!",
                    "You tried to steal in your neighbor's vault and got Caught!",
                    "You entered the bank's vault to steal bot got Caught!",
                };

                // Wallet String Array
                string[] listOfWallet = new string[]
                {
                    "You looked at your wallet and got: ",
                    "You check your mom's wallet and got: ",
                    "You found your old wallet and got: ",
                    "You steal someone's wallet and got: "
                };
                string[] listOfWalletfail = new string[]
                {
                    "You tried to steal in your mom's wallet but got Caught!",
                    "You tried to steal someone's wallet but got Caught!",
                    "You got robbed while trying to check your wallet!",
                };

                // Closet String Array
                string[] listOfCloset = new string[]
                {
                    "You searched the clothes in the closet and got: ",
                    "You searched the small boxes in the closet and got: ",
                    "You opened your old savings wallet in the closet and got: ",
                    "You grab your boxes on top of the closet and got: ",
                };

                // Attic String Array
                string[] listOfAttic = new string[]
                {
                    "You searched the old boxes at the attic and got: ",
                    "You searched the piggy bannk at the attic and got: ",
                    "You found an old wallet at the attic and got: ",
                    "You found a small box at the attic and got: ",
                };
                /*
                 * RANDOMIZER SECTION FOR THE LIST OF ARRAY STRINGS
                 */
                // Bed
                int randomBed = random.Next(0, listOfBed.Length);
                string bedSel = listOfBed[randomBed];
                // Vault
                int randomVault = random.Next(0, listOfVault.Length);
                string vaultSel = listOfVault[randomVault];
                int randomVault2 = random.Next(0, listOfVaultfail.Length);
                string vaultSel2 = listOfVaultfail[randomVault2];
                // Pocket
                int randomPock = random.Next(0, listOfPocket.Length);
                string pockSel = listOfPocket[randomPock];
                int randomPock2 = random.Next(0, listOfPocketfail.Length);
                string pockSel2 = listOfPocketfail[randomPock2];
                // Wallet
                int randomWallet = random.Next(0, listOfWallet.Length);
                string walletSel = listOfWallet[randomWallet];
                int randomWallet2 = random.Next(0, listOfWalletfail.Length);
                string walletSel2 = listOfWalletfail[randomWallet2];
                // Closet
                int randomCloset = random.Next(0, listOfCloset.Length);
                string closetSel = listOfCloset[randomCloset];
                // Attic
                int randomAttic = random.Next(0, listOfAttic.Length);
                string atticSel = listOfAttic[randomAttic];

                // SQL CONNECT
                SqlConnection connection1 = new SqlConnection(sqlClientACC);
                connection1.Open();

                // Select the user from database
                // If not in the database then add first before adding the value of money
                string userSelectDB = "Select * From UserBank Where UserID = '"+cmdUser+"'";
                SqlCommand selectUser = new SqlCommand(userSelectDB, connection1);
                SqlDataReader readUser = selectUser.ExecuteReader();

                // Just a input Filter to lowercase
                if (place != null)
                {
                    string place2 = place.ToLower();

                    // If user is in the Database
                    if (readUser.Read())
                    {
                        // Read the user Accon First
                        SqlConnection connection2 = new SqlConnection(sqlClientACC);
                        connection2.Open();
                        string runCMDD = "Select Bank, Wallet From UserBank Where UserID = '"+cmdUser+"'";
                        SqlCommand AddAcc = new SqlCommand(runCMDD, connection2);
                        SqlDataReader readAcc = AddAcc.ExecuteReader();

                        // Reads the user's account in the Database
                        while (readAcc.Read())
                        {
                            SqlConnection connection6 = new SqlConnection(sqlClientACC);
                            connection6.Open();
                            long currentBal = (long)readAcc["Wallet"];
                            long addedBal = currentBal + coinRandom;
                            long deductBal = currentBal - coinRandom;

                            // 2 separate command to add or deduct in the user's account
                            string runCommd = "UPDATE UserBank Set Wallet = '"+addedBal+"' Where UserID = '"+cmdUser+"'";
                            string runCommd2 = "UPDATE UserBank Set Wallet = '"+deductBal+"' Where UserID = '"+cmdUser+"'";


                            // Places Searched
                            if (place2 == "bed")
                            {
                                SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                commnd.ExecuteNonQuery();

                                var embed1 = new DiscordEmbedBuilder
                                {
                                    Title = bedSel + coinRandom + " coins!",
                                    Description = "New balance is: " + addedBal,
                                    Color = randomCol,
                                    Footer = new DiscordEmbedBuilder.EmbedFooter
                                    {
                                        Text = DateTime.Now.ToString("hh:mm tt"),
                                        IconUrl = null
                                    }
                                };
                                await ctx.RespondAsync(embed1);
                                connection6.Close();
                                return;
                            }
                            else if (place2 == "vault")
                            {
                                if (coinRandom >= 150)
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd2, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = vaultSel2,
                                        Description = "You lost: " + coinRandom + "\nNew balance is " + deductBal,
                                        Color = randomCol,
                                        Footer = new DiscordEmbedBuilder.EmbedFooter
                                        {
                                            Text = DateTime.Now.ToString("hh:mm tt"),
                                            IconUrl = null
                                        }
                                    };
                                    await ctx.RespondAsync(embed1);
                                    return;
                                }
                                else
                                {

                                    SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = vaultSel + coinRandom + " coins!",
                                        Description = "New balance is " + addedBal,
                                        Color = randomCol,
                                        Footer = new DiscordEmbedBuilder.EmbedFooter
                                        {
                                            Text = DateTime.Now.ToString("hh:mm tt"),
                                            IconUrl = null
                                        }
                                    };
                                    await ctx.RespondAsync(embed1);
                                }
                                connection6.Close();
                                return;
                            }
                            else if (place2 == "pocket")
                            {
                                if (coinRandom >= 150)
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd2, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = pockSel2,
                                        Description = "You lost: " + coinRandom + "\nNew balance is " + deductBal,
                                        Color = randomCol,
                                        Footer = new DiscordEmbedBuilder.EmbedFooter
                                        {
                                            Text = DateTime.Now.ToString("hh:mm tt"),
                                            IconUrl = null
                                        }
                                    };
                                    await ctx.RespondAsync(embed1);
                                    return;
                                }
                                else
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = pockSel + coinRandom + " coins!",
                                        Description = "New balance is: " + addedBal,
                                        Color = randomCol,
                                        Footer = new DiscordEmbedBuilder.EmbedFooter
                                        {
                                            Text = DateTime.Now.ToString("hh:mm tt"),
                                            IconUrl = null
                                        }
                                    };
                                    await ctx.RespondAsync(embed1);
                                }
                                connection6.Close();
                                return;
                            }
                            else if (place2 == "attic")
                            {
                                SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                commnd.ExecuteNonQuery();

                                var embed1 = new DiscordEmbedBuilder
                                {
                                    Title = atticSel + coinRandom + " coins!",
                                    Description = "New balance is: " + addedBal,
                                    Color = randomCol,
                                    Footer = new DiscordEmbedBuilder.EmbedFooter
                                    {
                                        Text = DateTime.Now.ToString("hh:mm tt"),
                                        IconUrl = null
                                    }
                                };
                                await ctx.RespondAsync(embed1);
                                connection6.Close();
                                return;
                            }
                            else if (place2 == "closet")
                            {
                                SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                commnd.ExecuteNonQuery();

                                var embed1 = new DiscordEmbedBuilder
                                {
                                    Title = closetSel + coinRandom + " coins!",
                                    Description = "New balance is: " + addedBal,
                                    Color = randomCol,
                                    Footer = new DiscordEmbedBuilder.EmbedFooter
                                    {
                                        Text = DateTime.Now.ToString("hh:mm tt"),
                                        IconUrl = null
                                    }
                                };
                                await ctx.RespondAsync(embed1);
                                connection6.Close();
                                return;
                            }
                            else if (place2 == "wallet")
                            {
                                if (coinRandom >= 150)
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd2, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = walletSel2,
                                        Description = "You lost: " + coinRandom + "\nNew balance is " + deductBal,
                                        Color = randomCol,
                                        Footer = new DiscordEmbedBuilder.EmbedFooter
                                        {
                                            Text = DateTime.Now.ToString("hh:mm tt"),
                                            IconUrl = null
                                        }
                                    };
                                    await ctx.RespondAsync(embed1);
                                    return;
                                }
                                else
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = walletSel + coinRandom + " coins!",
                                        Description = "New balance is: " + addedBal,
                                        Color = randomCol,
                                        Footer = new DiscordEmbedBuilder.EmbedFooter
                                        {
                                            Text = DateTime.Now.ToString("hh:mm tt"),
                                            IconUrl = null
                                        }
                                    };
                                    await ctx.RespondAsync(embed1);
                                }
                                connection6.Close();
                                return;
                            }
                            else
                            {
                                var embed1 = new DiscordEmbedBuilder
                                {
                                    Title = "This is not in the list of places that can be searched!",
                                    Description = "",
                                    Color = randomCol,
                                    Footer = new DiscordEmbedBuilder.EmbedFooter
                                    {
                                        Text = DateTime.Now.ToString("hh:mm tt"),
                                        IconUrl = null
                                    }
                                };
                                await ctx.RespondAsync(embed1);
                                return;
                            }
                        }
                        connection2.Close();
                        return;
                    }
                    // If user is not in the Database
                    else
                    {
                        // Set the user to the Database
                        int defaultBank = 500;
                        int defaultWallet = 500;
                        SqlConnection connection3 = new SqlConnection(sqlClientACC);
                        connection3.Open();

                        // Add the user to the database with free 1000 coins
                        string EditUser = "INSERT INTO UserBank(UserID,Bank,Wallet)values('"+cmdUser+"','"+defaultBank+"','"+defaultWallet+"')";
                        SqlCommand runCMD = new SqlCommand(EditUser, connection3);
                        runCMD.ExecuteNonQuery();
                        connection3.Close();


                        // Get the user's Current balance
                        SqlConnection connection4 = new SqlConnection(sqlClientACC);
                        connection4.Open();
                        string getUserBalance = "Select Bank, Wallet From UserBank Where UserID = '"+cmdUser+"'";
                        SqlCommand runCMD2 = new SqlCommand(getUserBalance, connection4);
                        SqlDataReader readUserBalance = runCMD2.ExecuteReader();

                        // Message the user if they are in the bank
                        while (readUserBalance.Read())
                        {
                            long walletBal = (long)readUserBalance["Wallet"];
                            long bankBal = (long)readUserBalance["Bank"];

                            // Message the user if they are in the bank which shows their balance
                            var embed1 = new DiscordEmbedBuilder
                            {
                                Title = "Your accout has been registered to the System!",
                                Description = "Bank: " + bankBal + " Wallet: " + walletBal,
                                Color = randomCol,
                                Footer = new DiscordEmbedBuilder.EmbedFooter
                                {
                                    Text = DateTime.Now.ToString("hh:mm tt"),
                                    IconUrl = null
                                }
                            };
                            await ctx.RespondAsync(embed1);
                        }
                        connection4.Close();
                        return;
                    }
                }
                else
                {
                    // Message the user didnt put a place to beg
                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "Specify a place to Search: ",
                        Description = "Bed, Vault, Pocket, Attic, Closet, Wallet",
                        Color = randomCol,
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            Text = DateTime.Now.ToString("hh:mm tt"),
                            IconUrl = null
                        }
                    };
                    await ctx.RespondAsync(embed1);
                }
                // SQL CONNECT CLOSE
                connection1.Close();
            }
            catch (Exception exe)
            {
                Console.WriteLine("Code Error!! " + exe);
            }
        }

        /*
         * WHY IS THIS EVEN HERE?
         */

        [Command("searchlist")]
        public async Task SearchList(CommandContext ctx)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var embed1 = new DiscordEmbedBuilder
            {
                Title = "Here is the list of places that can be searched!",
                Description = "Bed, Vault, Pocket, Attic, Closet, Wallet",
                Color = randomCol,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = DateTime.Now.ToString("hh:mm tt"),
                    IconUrl = null
                }
            };
            await ctx.RespondAsync(embed1);
        }

        /*
         * BALANCE COMMAND WELL YEAH YOU KNOW WHAT IT DOES
         */


        [Command("bal")]
        public async Task BalanceCommand(CommandContext ctx)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var cmdUser = ctx.User.Id;

            // SQL CONNECT
            SqlConnection connection1 = new SqlConnection(sqlClientACC);
            connection1.Open();

            // Select the user from database
            // If not in the database then add first before adding the value of money
            string userSelectDB = "Select * From UserBank Where UserID = '"+cmdUser+"'";
            SqlCommand selectUser = new SqlCommand(userSelectDB, connection1);
            SqlDataReader readUser = selectUser.ExecuteReader();
            if (readUser.Read())
            {
                // Read the user Account First
                SqlConnection connection2 = new SqlConnection(sqlClientACC);
                connection2.Open();
                string runCMDD = "Select Bank, Wallet From UserBank Where UserID = '"+cmdUser+"'";
                SqlCommand AddAcc = new SqlCommand(runCMDD, connection2);
                SqlDataReader readAcc = AddAcc.ExecuteReader();

                // Reads the user's account in the Database
                while (readAcc.Read())
                {
                    long walletBal = (long)readAcc["Wallet"];
                    long bankBal = (long)readAcc["Bank"];

                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "Your current balance is: ",
                        Description = "Wallet: " + walletBal + "\nBank: " + bankBal,
                        Color = randomCol,
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            Text = DateTime.Now.ToString("hh:mm tt"),
                            IconUrl = null
                        }
                    };
                    await ctx.RespondAsync(embed1);
                    return;
                }
            }
            else
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "You are not registered in the database!",
                    Description = "Use the beg command to register",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embed1);
                return;
            }
        }

        [Command("withdraw")]
        public async Task WithdrawCMD(CommandContext ctx)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            // First Check if the user is registered to the database

            var cmdUser = ctx.User.Id;

            SqlConnection connection1 = new SqlConnection(sqlClientACC);
            connection1.Open();
            string userSelectDB = "Select * From UserBank Where UserID = '"+cmdUser+"'";
            SqlCommand selectUser = new SqlCommand(userSelectDB, connection1);
            SqlDataReader readUser = selectUser.ExecuteReader();
            if (readUser.Read())
            {

            }
            else
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "You are not registered in the database!",
                    Description = "Use the beg command to register",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            connection1.Close();
        }

        [Command("deposit")]
        public async Task DepositCMD(CommandContext ctx)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            // First Check if the user is registered to the database

            var cmdUser = ctx.User.Id;

            SqlConnection connection1 = new SqlConnection(sqlClientACC);
            connection1.Open();
            string userSelectDB = "Select * From UserBank Where UserID = '"+cmdUser+"'";
            SqlCommand selectUser = new SqlCommand(userSelectDB, connection1);
            SqlDataReader readUser = selectUser.ExecuteReader();
            if (readUser.Read())
            {

            }
            else
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "You are not registered in the database!",
                    Description = "Use the beg command to register",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            connection1.Close();
        }
    }
}
