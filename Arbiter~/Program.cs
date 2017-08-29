using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;


namespace Arbiter
{
    public class botBody
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private IServiceProvider services;
        public static void Main(string[] args)
            => new botBody().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            commands = new CommandService();
            services = new ServiceCollection().BuildServiceProvider();
            client = new DiscordSocketClient();
        
            client.Log += Log;
            client.MessageReceived += MessageReceived;
            client.Ready += Ready;

            string token = "MzUwNTczMTY1Mjg3NTcxNDU2.DILqsw.-X-a4vOkuHOdhreu-vwvmn4mDHE";
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task Ready() //Выполняется, когда бот запущен    //Статус будет отображаться корректно, если ты будешь использовать !а Отключить, а не просто крашить бота.
        {
            var channel = client.GetChannel(351357679173894144) as SocketTextChannel;
            var status = await channel.GetMessagesAsync(1).Flatten();
            await channel.DeleteMessagesAsync(status);
            {
                EmbedBuilder Embed = new EmbedBuilder();
                Embed.WithColor(new Color(135, 206, 250));
                Embed.WithTitle("Запущен в данный момент");
                Embed.WithDescription("Введите **!Список**, чтобы просмотреть список команд\n*Версия: 0.1.2*");
                await channel.SendMessageAsync("", false, Embed);
            }
        }

        public async Task MessageReceived(SocketMessage message) //Выполняется, когда кто-то отправляет сообщение
        {
            if (message.Content == "!Список")
            {
                EmbedBuilder Embed = new EmbedBuilder();
                Embed.WithColor(new Color(135, 206, 250));
                Embed.WithTitle("Список комманд");
                Embed.WithDescription("**!a** - Показать список комманд для администратора \n**!Битва** - Сгенерировать случайного противника \n**!Проверка** - Проверить параметры противника");
                await message.Channel.SendMessageAsync("", false, Embed);
            }
            if (message.Content == "!а")
            {
                EmbedBuilder Embed = new EmbedBuilder();
                Embed.WithColor(new Color(135, 206, 250));
                Embed.WithTitle("Список комманд для пользователей с высшим доступом");
                Embed.WithDescription("**!a Отключить** - Отключает бота немедленно. Рекомендуется использовать для завершения работы.\n**!a Очистить** - Удаляет 100 последних сообщений в данном канале.\n**!a Очистить тихо** - Удаляет сообщения без вывода уведомления о удалении");
                await message.Channel.SendMessageAsync("", false, Embed);
            }
            if (message.Content == "!а Отключить" && message.Author.Id == 301316922530856960)
            {
                var channel = client.GetChannel(351357679173894144) as SocketTextChannel;
                var status = await channel.GetMessagesAsync(100).Flatten();
                await channel.DeleteMessagesAsync(status);
                await channel.SendMessageAsync("**Отключён в данный момент**\nКоманды не доступны.\n*Версия: 0.1.2*");
                CloseProgram(new Action(delegate{}));
            }
            if (message.Content == "!а Очистить" && message.Author.Id == 301316922530856960) 
            {
                var delete = await message.Channel.GetMessagesAsync(100).Flatten();
                await message.Channel.DeleteMessagesAsync(delete);
                await message.Channel.SendMessageAsync("Последние 100 сообщений были удалены.");
            }
            if (message.Content == "!а Очистить тихо" && message.Author.Id == 301316922530856960)
            {
                var delete = await message.Channel.GetMessagesAsync(100).Flatten();
                await message.Channel.DeleteMessagesAsync(delete);
            }
            if (message.Content == "!Битва")
                    {
                        string UserID = message.Author.Id.ToString();
                        Random ran = new Random();
                        int enemyType = ran.Next(1, 6);
                        int enemyLevel = ran.Next(1, 6);
                        SetEnemy Stats = new SetEnemy(UserID, enemyType, enemyLevel);
                        message.Channel.SendMessageAsync("Создан противник: " + Stats.enemyName + "\nУровень: " + Stats.enemyLevel.ToString() + "\nЖизнь: " + Stats.enemyHP.ToString() + "\nАтака: " + Stats.enemyAT.ToString() + " | Защита: " + Stats.enemyDF.ToString());
            }
            if (message.Content == "!Проверка")
            {
                SetEnemy Test = new SetEnemy();
                message.Channel.SendMessageAsync("Существо: " + Test.enemyName + "\nУровень: " + Test.enemyLevel.ToString() + "\nЖизнь: " + Test.enemyHP.ToString() + "\nАтака: " + Test.enemyAT.ToString() + " | Защита: " + Test.enemyDF.ToString());
            }
        }

        [Command("Заготовка для команды")]
        [Summary("Заготовка для команды")]
        public async Task Test2([Summary("Заготовка для команды")] IGuildUser user = null)
        {

        }

        private Task Log(LogMessage msg) //Логгирование
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public static void CloseProgram()
        {
            Process.GetCurrentProcess().Kill();
        }

        public static void CloseProgram(Action actionBeforeClosing)
        {
            CloseProgram();
        }
    }

    struct SetEnemy
    {
        public string UserID;
        public int enemyType;
        public int enemyLevel;
        public string enemyName;
        public int enemyBasicHP;
        public int enemyBasicAT;
        public int enemyBasicDF;
        public int enemyBasicCT;
        public int enemyBasicEV;
        public int enemyHP;
        public int enemyAT;
        public int enemyDF;
        public int enemyCT;
        public int enemyEV;

        public SetEnemy(string UserID, int enemyType, int enemyLevel)
        {
            this.UserID = UserID;
            this.enemyType = enemyType;
            this.enemyLevel = enemyLevel;
            enemyName = "";
            enemyBasicHP = 0;
            enemyBasicAT = 0;
            enemyBasicDF = 0;
            enemyBasicCT = 0;
            enemyBasicEV = 0;
            if (enemyType == 1) { enemyName = "Имя 1"; enemyBasicHP = 30; enemyBasicAT = 10; enemyBasicDF = 1; enemyBasicCT = 1; enemyBasicEV = 10; }
            if (enemyType == 2) { enemyName = "Имя 2"; enemyBasicHP = 15; enemyBasicAT = 15; enemyBasicDF = 1; enemyBasicCT = 2; enemyBasicEV = 5; }
            if (enemyType == 3) { enemyName = "Имя 3"; enemyBasicHP = 20; enemyBasicAT = 10; enemyBasicDF = 1; enemyBasicCT = 3; enemyBasicEV = 15; }
            if (enemyType == 4) { enemyName = "Имя 4"; enemyBasicHP = 1; enemyBasicAT = 0; enemyBasicDF = 0; enemyBasicCT = 0; enemyBasicEV = 80; }
            if (enemyType == 5) { enemyName = "Имя 5"; enemyBasicHP = 10; enemyBasicAT = 10; enemyBasicDF = 1; enemyBasicCT = 2; enemyBasicEV = 35; }
            enemyHP = (int)Math.Round((double)enemyBasicHP * enemyLevel);
            enemyAT = (int)Math.Round((double)enemyBasicAT * enemyLevel);
            enemyDF = (int)Math.Round((double)enemyBasicDF * enemyLevel);
            enemyCT = enemyBasicCT;
            enemyEV = enemyBasicEV;
        }
    }
}