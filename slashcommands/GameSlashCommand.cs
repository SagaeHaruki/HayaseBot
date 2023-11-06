using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HayaseBot.slashcommands
{
    public class GameSlashCommand : ApplicationCommandModule
    {
        // This is the sql data client ID
        string sqlClientACC = "Data Source=HAYASEYUUKA\\SQLEXPRESS;Initial Catalog=DiscordStorage;Integrated Security=True";

        private Random random = new Random();

        /*
         * EASIEST OF THEM ALL THE BEG COMMAND
         */

        [SlashCommand("beg", "Begs for some money")]
        [SlashCooldown(1, 17, SlashCooldownBucketType.User)]
        public async Task BegCommand(InteractionContext ctx)
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
                            Timestamp = DateTime.UtcNow
                        };
                        await ctx.CreateResponseAsync(embed1);
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
                            Timestamp = DateTime.UtcNow
                        };
                        await ctx.CreateResponseAsync(embed1);
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
                        Timestamp = DateTime.UtcNow
                    };
                    await ctx.CreateResponseAsync(embed1);
                }
                connection4.Close();
                connection1.Close();
                return;
            } // Ends Here
        }
    }
}
