// See https://aka.ms/new-console-template for more information

ReadFile();
Console.ReadKey();

static void ReadFile()
{
    string file = @"C:\Users\digis\OneDrive\Documents\Mayte Chavez Salazar\LayoutUsuario.txt";
    if(File.Exists(file))//Si existe el archivo en la ruta
    {
        StreamReader textFile = new StreamReader(file);
        string line;
        line = textFile.ReadLine();

        while((line = textFile.ReadLine()) != null) //Mientras existan lineas por leer
        {
            string[] lines = line.Split('|'); //Del archivo .txt Split va a quitar todos los '|' que existan

            ML.Usuario usuario = new ML.Usuario();

            usuario.NombreUsuario = lines[1];
            usuario.ApellidoPaternoU = lines[2];
            usuario.ApellidoMaternoU = lines[3];   
            usuario.FechaNacimiento = lines[4];
            usuario.Genero = lines[5];
            usuario.Password = lines[6];
            usuario.Telefono = lines[7];
            usuario.Celular = lines[8]; 
            usuario.Curp = lines[9];    
            usuario.UserName = lines[10];
            usuario.Email = lines[11];

            usuario.Rol = new ML.Rol(); //Se instancia o inicializa para poder utilizar sus propiedades
            usuario.Rol.IdRol = byte.Parse(lines[12]);

            usuario.Imagen = null;

            usuario.Direccion = new ML.Direccion(); //Se instancia para poder usar sus propiedades

            usuario.Direccion.Calle = lines[13];
            usuario.Direccion.NumeroInterior = lines[14];
            usuario.Direccion.NumeroExterior = lines[15];

            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.IdColonia = int.Parse(lines[16]);

            ML.Result result = BL.Usuario.Add(usuario);

            if (result.Correct)
            {
                Console.WriteLine("Correcto");
                Console.ReadKey();
            }
            //else
            //{

            //}


        }
    }
}
