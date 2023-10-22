using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayaseBot.commands
{
    public class BankCommands : BaseCommandModule
    {
        string sqlClientACC = "Data Source=HAYASEYUUKA\\SQLEXPRESS;Initial Catalog=DiscordStorage;Integrated Security=True";

        private Random random = new Random();

        [Command("withdraw")]
        public async Task WithdrawCMD(CommandContext ctx, int amt)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            // First Check if the user is registered to the database

            var cmdUser = ctx.User.Id;
            var UserInput = amt;

            SqlConnection connection1 = new SqlConnection(sqlClientACC);
            connection1.Open();
            string userSelectDB = "Select * From UserBank Where UserID = '"+cmdUser+"'";
            SqlCommand selectUser = new SqlCommand(userSelectDB, connection1);
            SqlDataReader readUser = selectUser.ExecuteReader();

            if (readUser.Read())
            {
                // Section that will read the User's Account Data
                SqlConnection connection2 = new SqlConnection(sqlClientACC);
                connection2.Open();
                string runCMDD = "Select Bank, Wallet From UserBank Where UserID = '"+cmdUser+"'";
                SqlCommand AddAcc = new SqlCommand(runCMDD, connection2);
                SqlDataReader readAcc = AddAcc.ExecuteReader();

                while (readAcc.Read())
                {
                    SqlConnection connection6 = new SqlConnection(sqlClientACC);
                    connection6.Open();
                    long currentBank = (long)readAcc["Bank"];
                    long currentWallet = (long)readAcc["Wallet"];
                    // FIRST Remove in the bank account the amt
                    long newBankBal = currentBank - UserInput;
                    // Add it to wallet
                    long newWalletBal = currentWallet + UserInput;


                    /*
                     * Checks if user input is morethan or equal to current Bank balance
                     */
                    if (currentBank >= UserInput)
                    {
                        string runCMD = "UPDATE UserBank Set Bank = '"+newBankBal+"', Wallet = '"+newWalletBal+"' Where UserID = '"+cmdUser+"'";

                        SqlCommand commnd = new SqlCommand(runCMD, connection6);
                        commnd.ExecuteNonQuery();

                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = "You withdraw a total of: " + UserInput,
                            Description = "Your new Balance is: \nWallet: " + newWalletBal + "\nBank: " + newBankBal,
                            Color = randomCol,
                            Timestamp = DateTime.UtcNow,
                        };
                        await ctx.RespondAsync(embed1);
                        connection6.Close();
                        return;
                    }
                    else 
                    {
                        /*
                         * If the user does not have enough money then it will do nothing but send a message
                         */
                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = "You don't have enough money in your Bank Account",
                            Color = randomCol,
                            Timestamp = DateTime.UtcNow,
                        };
                        await ctx.RespondAsync(embed1);
                        connection6.Close();
                        return;
                    }
                }
            }
            else
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "You are not registered in the database!",
                    Description = "Use the beg command to register",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow,
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            connection1.Close();
        }

        [Command("deposit")]
        public async Task DepositCMD(CommandContext ctx, int amt)
        {
            // Color Randomizer
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            // First Check if the user is registered to the database

            var cmdUser = ctx.User.Id;
            var UserInput = amt;

            SqlConnection connection1 = new SqlConnection(sqlClientACC);
            connection1.Open();
            string userSelectDB = "Select * From UserBank Where UserID = '"+cmdUser+"'";
            SqlCommand selectUser = new SqlCommand(userSelectDB, connection1);
            SqlDataReader readUser = selectUser.ExecuteReader();
            if (readUser.Read())
            {
                // Section that will read the User's Account Data
                SqlConnection connection2 = new SqlConnection(sqlClientACC);
                connection2.Open();
                string runCMDD = "Select Bank, Wallet From UserBank Where UserID = '"+cmdUser+"'";
                SqlCommand AddAcc = new SqlCommand(runCMDD, connection2);
                SqlDataReader readAcc = AddAcc.ExecuteReader();

                while (readAcc.Read())
                {
                    SqlConnection connection6 = new SqlConnection(sqlClientACC);
                    connection6.Open();
                    long currentBank = (long)readAcc["Bank"];
                    long currentWallet = (long)readAcc["Wallet"];
                    // First Remove from wallet the amt
                    long newWalletBal = currentWallet - UserInput;
                    // Then add it to bank
                    long newBankBal = currentBank + UserInput;

                    /*
                     * Checks if user input is morethan or equal to current Wallet balance
                     */
                    if (currentWallet >= UserInput)
                    {
                        string runCMD = "UPDATE UserBank Set Bank = '"+newBankBal+"', Wallet = '"+newWalletBal+"' Where UserID = '"+cmdUser+"'";

                        SqlCommand commnd = new SqlCommand(runCMD, connection6);
                        commnd.ExecuteNonQuery();

                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = "You Deposit a total of: " + UserInput,
                            Description = "Your new Balance is: \nWallet: " + newWalletBal + "\nBank: " + newBankBal,
                            Color = randomCol,
                            Timestamp = DateTime.UtcNow,
                        };
                        await ctx.RespondAsync(embed1);
                        connection6.Close();
                        return;
                    }
                    else
                    {
                        /*
                         * If the user does not have enough money then it will do nothing but send a message
                         */
                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = "You don't have enough money in your Wallet",
                            Color = randomCol,
                            Timestamp = DateTime.UtcNow,
                        };
                        await ctx.RespondAsync(embed1);
                        connection6.Close();
                        return;
                    }
                }
            }
            else
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "You are not registered in the database!",
                    Description = "Use the beg command to register",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            connection1.Close();
        }

        /*
         * Pay Command Does what it says, Pay the person mentioned
         */
        [Command("pay")]
        public async Task PayCommand(CommandContext ctx, DiscordMember target = null, [RemainingText] int amt = 0)
        {
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);
            var cmdUser = ctx.User.Id;

            // If target is the command user
            if (cmdUser == target.Id)
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "You cannot pay yourself!",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            // If target is the bot
            else if (target.Id == 1033001102687346718)
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "You don't have to pay me!",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            // If user didn't specify an amount
            else if (amt == 0)
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "Enter the amount to pay!",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            // If user tried to enter a negative value
            else if (amt <= 0)
            {
                var embed1 = new DiscordEmbedBuilder
                {
                    Title = "Enter a valid amount!",
                    Color = randomCol,
                    Timestamp = DateTime.UtcNow
                };
                await ctx.RespondAsync(embed1);
                return;
            }
            else
            {

                /*
                 * Run checks if user is in the database
                 */

                SqlConnection connection1 = new SqlConnection(sqlClientACC);
                connection1.Open();
                string userSelectDB = "Select * From UserBank Where UserID = '"+cmdUser+"'";
                SqlCommand selectUser = new SqlCommand(userSelectDB, connection1);
                SqlDataReader readUser = selectUser.ExecuteReader();
                if (readUser.Read())
                {
                    SqlConnection connection2 = new SqlConnection(sqlClientACC);
                    connection2.Open();
                    string runCMDD = "Select Bank, Wallet From UserBank Where UserID = '"+cmdUser+"'";
                    SqlCommand AddAcc = new SqlCommand(runCMDD, connection2);
                    SqlDataReader readAcc = AddAcc.ExecuteReader();

                    while (readAcc.Read())
                    {
                        /*
                         * Run checks if target is in database
                         */

                        // This is the user's Wallet
                        long User_currentWallet = (long)readAcc["Wallet"];

                        if (User_currentWallet >= amt)
                        {
                            SqlConnection connection3 = new SqlConnection(sqlClientACC);
                            connection3.Open();
                            string userSelectDB1 = "Select * From UserBank Where UserID = '"+target.Id+"'";
                            SqlCommand selectUser1 = new SqlCommand(userSelectDB1, connection3);
                            SqlDataReader readUser1 = selectUser1.ExecuteReader();
                            if (readUser1.Read())
                            {
                                SqlConnection connection4 = new SqlConnection(sqlClientACC);
                                connection4.Open();
                                string runCMDD1 = "Select Bank, Wallet From UserBank Where UserID = '"+target.Id+"'";
                                SqlCommand AddAcc1 = new SqlCommand(runCMDD1, connection4);
                                SqlDataReader readAcc1 = AddAcc1.ExecuteReader();

                                while (readAcc1.Read())
                                {
                                    SqlConnection connection6 = new SqlConnection(sqlClientACC);
                                    SqlConnection connection7 = new SqlConnection(sqlClientACC);
                                    connection6.Open();
                                    connection7.Open();

                                    // Add the total to a new value
                                    long Target_curWallet = (long)readAcc1["Wallet"];
                                    long newTarg_wallBal = Target_curWallet + amt;
                                    long newUser_wallBal = User_currentWallet - amt;

                                    string runCMD = "UPDATE UserBank Set Wallet = '"+newTarg_wallBal+"' Where UserID = '"+target.Id+"'";
                                    string runCMD2 = "UPDATE UserBank Set Wallet = '"+newUser_wallBal+"' Where UserID = '"+cmdUser+"'";
                                    SqlCommand commnd = new SqlCommand(runCMD, connection6);
                                    SqlCommand commnd2 = new SqlCommand(runCMD2, connection7);
                                    commnd2.ExecuteNonQuery();
                                    commnd.ExecuteNonQuery();

                                    var embed1 = new DiscordEmbedBuilder
                                    {
                                        Title = "Hayase Payment System!",
                                        Description = ctx.User.Username + " payed: " + target.Username + " \nwith amount of: " + amt,
                                        Color = randomCol,
                                        Timestamp = DateTime.UtcNow
                                    };
                                    await ctx.RespondAsync(embed1);
                                    connection6.Close();
                                    connection7.Close();
                                    return;
                                }
                                connection4.Close();
                                return;
                            }
                            else
                            {
                                var embed1 = new DiscordEmbedBuilder
                                {
                                    Title = "You are not in the Database!Target is not in the Database!",
                                    Color = randomCol,
                                    Timestamp = DateTime.UtcNow
                                };
                                await ctx.RespondAsync(embed1);
                                return;
                            }
                        }
                        else
                        {
                            var embed1 = new DiscordEmbedBuilder
                            {
                                Title = "You don't have enough balance!",
                                Color = randomCol,
                                Timestamp = DateTime.UtcNow
                            };
                            await ctx.RespondAsync(embed1);
                            return;
                        }
                    }
                    connection2.Close();
                    return;
                }
                else
                {
                    var embed1 = new DiscordEmbedBuilder
                    {
                        Title = "You are not in the Database!",
                        Color = randomCol,
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.RespondAsync(embed1);
                    connection1.Close();
                    return;
                }
            }
        }
    }
}
