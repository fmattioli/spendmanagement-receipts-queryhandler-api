DateTime dataHoje = DateTime.UtcNow;
DateTime inicioDeMaio = new DateTime(2023, 05, 01);

int totalDeDomingos = (1 + dataHoje.Subtract(inicioDeMaio).Days + (6 + (int)inicioDeMaio.DayOfWeek) % 7) / 7;

if(totalDeDomingos == 2)
{
	for (int i = 0; i < 20; i++)
	{
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("FELIZ DIAS DAS MÃES!!!!!");
    }

    Console.ReadLine();
}