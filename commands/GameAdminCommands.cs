using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HayaseBot.commands
{
    public class GameAdminCommands : BaseCommandModule
    {
        string sqlClientACC = "Data Source=HAYASEYUUKA\\SQLEXPRESS;Initial Catalog=DiscordStorage;Integrated Security=True";

        private Random random = new Random();
        /*
         * Give but wallet version
         */
        [Command("give")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task TestingCommand(CommandContext ctx, DiscordMember target = null, int amt = 0)
        {
            Console.WriteLine(target);

            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            DiscordColor randomCol = new DiscordColor((byte)red, (byte)green, (byte)blue);

            /*
             * If command user didn's specify a target
             */
            if (target == null)
            {
                var embedMessage = new DiscordEmbedBuilder()
                {
                    Title = "Please Specify a target User!",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embedMessage);
                return;
            }
            /*
             * If command user did try to give the bot (Hayase Bot) the Coins
             */
            else if (target.Id == 1033001102687346718)
            {
                var embedMessage = new DiscordEmbedBuilder()
                {
                    Title = "I'm a millionaire already i don't need that",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embedMessage);
                return;
            }
            /*
             * If user didn't specify the amount to give
             */
            else if (amt == 0)
            {
                var embedMessage = new DiscordEmbedBuilder()
                {
                    Title = "Please enter a decent amount!",
                    Color = randomCol,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = DateTime.Now.ToString("hh:mm tt"),
                        IconUrl = null
                    }
                };
                await ctx.RespondAsync(embedMessage);
                return;
            }
            /*
             * Run's the whole code
             */
            else
            {
                SqlConnection sql_con = new SqlConnection(sqlClientACC);
                sql_con.Open();

                string userSelectDB = "Select * From UserBank Where UserID = '"+target.Id+"'";
                SqlCommand selectUser = new SqlCommand(userSelectDB, sql_con);
                SqlDataReader readUser = selectUser.ExecuteReader();
                /*
                 * Run checks in the the database if user is in
                 */
                if (readUser.Read())
                {
                    /*
                     * If user is in, then it will check for its Bank and wallet
                     */
                    SqlConnection connection2 = new SqlConnection(sqlClientACC);
                    connection2.Open();
                    string runCMDD = "Select Bank, Wallet From UserBank Where UserID = '"+target.Id+"'";
                    SqlCommand AddAcc = new SqlCommand(runCMDD, connection2);
                    SqlDataReader readAcc = AddAcc.ExecuteReader();

                    while (readAcc.Read())
                    {
                        SqlConnection connection6 = new SqlConnection(sqlClientACC);
                        connection6.Open();

                        long currBal = (long)readAcc["Wallet"];

                        long newWalBal = currBal + amt;

                        string runCMD = "UPDATE UserBank Set Wallet = '"+newWalBal+"' Where UserID = '"+target.Id+"'";

                        SqlCommand commnd = new SqlCommand(runCMD, connection6);
                        commnd.ExecuteNonQuery();

                        var embedMessage = new DiscordEmbedBuilder()
                        {
                            Title = "Given " + target.DisplayName + " user " + amt + " ",
                            Color = randomCol,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = DateTime.Now.ToString("hh:mm tt"),
                                IconUrl = null
                            }
                        };
                        await ctx.RespondAsync(embedMessage);
                        connection6.Close();
                        return;
                    }
                    connection2.Close();
                    return;
                }
                /*
                 * If user is not yet in the database
                 */
                else
                {
                    var embedMessage = new DiscordEmbedBuilder()
                    {
                        Title = "Cannot find user",
                        Color = randomCol,
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            Text = DateTime.Now.ToString("hh:mm tt"),
                            IconUrl = null
                        }
                    };
                    await ctx.RespondAsync(embedMessage);
                }    
                sql_con.Close();
                return;
            }
        }
    }
}
