using MySqlConnector;

namespace torpedo
{
    public partial class Form1 : Form
    {
        MySqlConnection? connection;
        public Form1()
        {
            this.connection = null;

            InitializeComponent();

            for (int i =0; i<10; i++)
            {
                for (int j =0; j<10; j++)
                {
                    TengerGomb buttonk = new TengerGomb();

                    buttonk.Location = new Point(50 + 25*j, 110+ 25*i);
                    buttonk.Name = "button_"+i+"_"+j;
                    buttonk.Size = new Size(23, 23);
                    buttonk.TabIndex = 0;
                    buttonk.Text = "";
                    buttonk.UseVisualStyleBackColor = true;
                    buttonk.Click += buttonk_Click;

                    buttonk.oszlop = j;
                    buttonk.sor = i;

                    Controls.Add(buttonk);
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;

            // set these values correctly for your database server
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "",
                Database = "13c_torpedo",
            };

            // open a connection asynchronously
            this.connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            // create a DB command and set the SQL statement with parameters
            using var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO jatekosok (`id`, `nev`) VALUES (NULL, '{s}');";

            /*
            // execute the command and read the results
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var id = reader.GetInt32("order_id");
                var date = reader.GetDateTime("order_date");
                // ...
            }
            connection.Close();
            INSERT ,ALTER, UPDATE, DELETE stb: ExevuteNonQuery
            */
            await command.ExecuteNonQueryAsync();

            label1.Text = "Az adatbázis-kapcsolat létrejött.";
        }

        private async void buttonk_Click(object sender, EventArgs e)
        {
            TengerGomb mire_kattintottam = (TengerGomb)sender;
            int sor = mire_kattintottam.sor;
            int oszlop = mire_kattintottam.oszlop;
            int? talalat = null;

            if (this.connection == null) return;

            using var command = this.connection.CreateCommand();
            command.CommandText = $"SELECT id FROM hajok WHERE (ksor=vsor AND ksor={sor} AND koszlop <= {oszlop} AND {oszlop} <= voszlop) OR (koszlop=voszlop AND koszlop={oszlop} AND ksor <= {sor} AND {sor} <= vsor);";

            // execute the command and read the results
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                talalat = reader.GetInt32("id");
                // ...
            }
            if(talalat== null)
            {
                mire_kattintottam.Text = "X";
            }
            else
            {
                mire_kattintottam.Text=talalat.ToString();
                mire_kattintottam.BackColor = Color.Red;
            }
        }
    }
    public class TengerGomb : Button
    {
        public int oszlop;
        public int sor;
    }
}
