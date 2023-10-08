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

        [Command("beg")]
        public async Task BegCommand(CommandContext ctx)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var cmdUser = ctx.User.Id;
            int coinRandom = random.Next(50, 201);

            var embed1 = new DiscordEmbedBuilder
            {
                Title = "You begged and got " + coinRandom + " Coins",
                Color = randomCol,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = DateTime.Now.ToString("hh:mm tt"),
                    IconUrl = null
                }
            };
            await ctx.RespondAsync(embed1);
        }

        [Command("search")]
        [Cooldown(1, 36, CooldownBucketType.User)]
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
                int coinRandom = random.Next(10, 201);
               
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
                                    Title = "You searched the bed and found " + coinRandom + " coins!",
                                    Description = "New balance is " + addedBal,
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
                                if (coinRandom <= 150)
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd2, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = "You searched a person's pocket and got Caught!",
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
                                else
                                {

                                    SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = "You searched the bed and found " + coinRandom + " coins!",
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
                                if (coinRandom <= 150)
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd2, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = "You searched a person's pocket and got Caught!",
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
                                else
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = "You searched a person's pocket and got " + coinRandom + " coins!",
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
                            else if (place2 == "attic")
                            {
                                SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                commnd.ExecuteNonQuery();

                                var embed1 = new DiscordEmbedBuilder
                                {
                                    Title = "You searched the Attic and found " + coinRandom + " coins!",
                                    Description = "New balance is " + addedBal,
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
                                    Title = "You searched the Closet and found " + coinRandom + " coins!",
                                    Description = "New balance is " + addedBal,
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
                                if (coinRandom <= 150)
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd2, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = "You searched a person's wallet and got Caught!",
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
                                else
                                {
                                    SqlCommand commnd = new SqlCommand(runCommd, connection6);
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = "You searched a person's wallet and got " + coinRandom + " coins!",
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
    }
}
