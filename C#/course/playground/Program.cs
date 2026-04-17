namespace Program;

class Program
{
	public static void Main()
	{
		int[] readings = [1, 4, 0, -2345, 6, 123, -4, -999, 456, 8, 3];

	}

	double Avg(int[] readings)
	{
		int sum = 0;
		int n = 0;

		for (int i = 0; i < readings.length; i++)
		{
			if (readings[i] == -999)
			{
				break;
			}

			if (readings[i] >= 0)
			{
				sum += readings[i];
				n++;
			}
		}

		return sum / n;
	}
}
