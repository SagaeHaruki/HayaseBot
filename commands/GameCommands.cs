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


            // Read User Starts here
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
            } // Ends Here
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

        /*
         * Slot Machine Command
         */
        [Command("slotmachine")]
        public async Task SlotMachineCMD(CommandContext ctx)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            var cmdUser = ctx.User.Id;

            // Slot machine randomizer

            string[] items = { "💸", "🍉", "❌", "🍋 ", "♥️", "🍒" };
            int randomIndex1 = random.Next(items.Length);
            int randomIndex2 = random.Next(items.Length);
            int randomIndex3= random.Next(items.Length);
            string rand1 = items[randomIndex1];
            string rand2 = items[randomIndex2];
            string rand3 = items[randomIndex3];

            /*
             * NOTE: There have been slight change on how the slotmachine will function
             * instead of user betting, they are being changed by 1000 to play slot machine
             * Prices will differ, but more likely to lose the bet.
             */


            // SQL CONNECT
            SqlConnection connection1 = new SqlConnection(sqlClientACC);
            connection1.Open();

            // Select the user from database
            // If not in the database then add first before adding the value of money
            string userSelectDB = "Select * From UserBank Where UserID = '"+cmdUser+"'";
            SqlCommand selectUser = new SqlCommand(userSelectDB, connection1);
            SqlDataReader readUser = selectUser.ExecuteReader();


            // Read User Start here
            if (readUser.Read())
            {
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
                    // List of winning that will be used in SQL Query
                    long tCashWin = currentBal + 2000;
                    long tCherryWin = currentBal + 3000;
                    long tMelon = currentBal + 1500;
                    long lost = currentBal - 1000;

                    if (currentBal >= 1000)
                    {
                        /*
                         * RANDOM: 3 CASH, Prize: +2000
                         */
                         
                        if (rand1 == "💸" && rand2 == "💸" && rand3 == "💸")
                        {
                            // Double the amt

                            var embed1 = new DiscordEmbedBuilder
                            {
                                Title = "SLOT MACHINE!",
                                Description = rand1 + "  " + rand2 + "  " + rand3 + "\nYou win!",
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
                        /*
                         * RANDOM: 3 Cherry, Prize: +5000
                         */
                        else if (rand1 == "🍒" && rand2 == "🍒" && rand3 == "🍒")
                        {
                            var embed1 = new DiscordEmbedBuilder
                            {
                                Title = "SLOT MACHINE!",
                                Description = rand1 + "  " + rand2 + "  " + rand3 + "\nYou win!",
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
                        /*
                         * RANDOM: 3 Melon, Prize: +1500
                         */
                        else if (rand1 == "🍉" && rand2 == "🍉" && rand3 == "🍉")
                        {
                            var embed1 = new DiscordEmbedBuilder
                            {
                                Title = "SLOT MACHINE!",
                                Description = rand1 + "  " + rand2 + "  " + rand3 + "\nYou win!",
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
                        /*
                         * More input here will be implemented
                         */
                        else
                        {
                            // Lost = amt input
                            var embed1 = new DiscordEmbedBuilder
                            {
                                Title = "SLOT MACHINE!",
                                Description = rand1 + "  " + rand2 + "  " + rand3 + "\nYOU LOST!",
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
                            Title = "You don't have enough balance",
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

                } // While Loop Ends Here

            } // Ends Here
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
    }
}
