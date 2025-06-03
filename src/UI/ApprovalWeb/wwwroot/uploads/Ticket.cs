
    public abstract class Ticket {
        private static Random rng = new Random();
        public string? CustomerName { get; set; }
        public int TicketNumber { get; set; }
        public DateTime DepartDate { get; set; }

        public abstract void ShowInfo();
        public static int GetRandomVehicleNumber() {
            return rng.Next(1,12);
        }
    }

