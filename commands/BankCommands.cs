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
                    else 
                    {
                        /*
                         * If the user does not have enough money then it will do nothing but send a message
                         */
                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = "You don't have enough money in your Bank Account",
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
                    else
                    {
                        /*
                         * If the user does not have enough money then it will do nothing but send a message
                         */
                        var embed1 = new DiscordEmbedBuilder
                        {
                            Title = "You don't have enough money in your Wallet",
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
