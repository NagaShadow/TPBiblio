using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace _35._1_TP_Biblio
{
    class Program
    {
        static void Main(string[] args)
        {
            NpgsqlConnection maCnx;
            NpgsqlConnection maCnx2;// ! déclaration avant le bloc Try
            NpgsqlDataReader jeuEnr = null;
            NpgsqlDataReader jeuEnr2 = null;
            maCnx = new NpgsqlConnection("Server=127.0.0.1;Port=5432;" + "User Id=postgres;Password=postgres;Database=Biblio;");
            maCnx2 = new NpgsqlConnection("Server=127.0.0.1;Port=5432;" + "User Id=postgres;Password=postgres;Database=Biblio;");
            string requête, requête2;
            int choix;
            do
            {
                Console.WriteLine("1. Afficher tous les noms des éditeurs.\n2. Afficher tous les enregistrements d’une table. \n3. Ajouter un auteur dans Authors");
                choix = int.Parse(Console.ReadLine());
                switch (choix)
                {
                    case 1:
                        try
                        { 
                            maCnx.Open(); // on se connecte
                            Console.WriteLine("Etat de la connexion : " + maCnx.State.ToString());

                            Console.WriteLine();
                            requête = "Select name from publishers";
                            var maCde = new NpgsqlCommand(requête, maCnx);
                            jeuEnr = maCde.ExecuteReader();
                            while (jeuEnr.Read())
                            {
                                Console.Write(jeuEnr["name"] + "\t");
                                Console.WriteLine();
                            }
                            Console.WriteLine("Press Any to continue ...");
                            Console.ReadKey();
                        }
                        catch (NpgsqlException e)
                        {
                            Console.WriteLine("Erreur " + e.ToString());
                        }
                        finally
                        {
                            if (jeuEnr is object & !jeuEnr.IsClosed)
                            {
                                jeuEnr.Close(); // s'il existe et n'est pas déjà fermé
                            }
                            if (maCnx is object & maCnx.State == ConnectionState.Open)
                            {
                                maCnx.Close(); // on se déconnecte
                            }
                        }
                        break;
                    case 2:
                        try
                        {
                            maCnx.Open(); // on se connecte
                            Console.WriteLine("Nom de la Table ?");
                            string Table = Console.ReadLine();
                            requête2 = "Select * from " + Table;
                            var maCde2 = new NpgsqlCommand(requête2, maCnx);
                            jeuEnr = maCde2.ExecuteReader();
                            for (int i = 0; i < jeuEnr.FieldCount; i++)
                            {
                                Console.Write(jeuEnr.GetName(i) + "\t");
                            }
                            while (jeuEnr.Read())
                            {
                                for (int i = 0; i < jeuEnr.FieldCount; i++)
                                {
                                    Console.Write(jeuEnr[jeuEnr.GetName(i)] + "\t");
                                }
                                Console.WriteLine();
                            }
                            Console.ReadLine();
                        }
                        catch (NpgsqlException e)
                        {
                            Console.WriteLine("Erreur " + e.ToString());
                        }
                        finally
                        {
                            if (jeuEnr is object & !jeuEnr.IsClosed)
                            {
                                jeuEnr.Close(); // s'il existe et n'est pas déjà fermé
                            }
                            if (maCnx is object & maCnx.State == ConnectionState.Open)
                            {
                                maCnx.Close(); // on se déconnecte
                            }
                        }
                        break;
                    case 3:
                        try
                        {
                            maCnx.Open();
                            Console.WriteLine("Nom de L'auteur ?");
                            string Auteur = Console.ReadLine();
                            Console.WriteLine("Année de naissance ?");
                            string annéenaissance = Console.ReadLine();
                            string requête3 = "Insert into authors(AUTHOR, year_born) values ('" + Auteur + "','" + annéenaissance + "')";
                            var maCde3 = new NpgsqlCommand(requête3, maCnx);
                            int nbLigneAffectées;
                            nbLigneAffectées = maCde3.ExecuteNonQuery();

                            Console.WriteLine("Nombre de ligne affectée(s) :" + nbLigneAffectées.ToString());
                            Console.ReadLine();
                        }
                        catch (NpgsqlException e)
                        {
                            Console.WriteLine("Erreur " + e.ToString());
                        }
                        finally
                        {
                            if (maCnx is object & maCnx.State == ConnectionState.Open)
                            {
                                maCnx.Close(); // on se déconnecte
                            }
                        }
                            Console.ReadLine();
                        break;
                    case 4:
                        try
                        {
                            maCnx.Open();
                            Console.WriteLine("Numero du livre (ISBN) ?");
                            string isbn = Console.ReadLine();
                            string requête4 = "DELETE FROM titleauthor WHERE isbn = '" + isbn + "'; DELETE FROM titles WHERE isbn = '" + isbn +"'";
                            var maCde3 = new NpgsqlCommand(requête4, maCnx);
                            int nbLigneAffectées;
                            nbLigneAffectées = maCde3.ExecuteNonQuery();

                            Console.WriteLine("Nombre de ligne affectée(s) :" + nbLigneAffectées.ToString());
                            Console.WriteLine(requête4);
                            Console.ReadLine();
                        }
                        catch (NpgsqlException e)
                        {
                            Console.WriteLine("Erreur " + e.ToString());
                        }
                        finally
                        {
                            if (maCnx is object & maCnx.State == ConnectionState.Open)
                            {
                                maCnx.Close(); // on se déconnecte
                            }
                        }
                        break;
                    case 5:
                        try
                        {
                            maCnx.Open();
                            Console.WriteLine("Entrée votre Titre");
                            string titre = Console.ReadLine();
                            Console.WriteLine("Entrée votre ISBN");
                            string isbn = Console.ReadLine();
                            Console.WriteLine("Entrée un Auteur");
                            string auteur = Console.ReadLine();
                            Console.WriteLine("Entrée un Éditeur");
                            string editeur = Console.ReadLine();

                            requête = "SELECT au_id FROM authors WHERE author = '" + auteur + "'";
                            var maCde = new NpgsqlCommand(requête, maCnx);
                            Int32 auid = Convert.ToInt32(maCde.ExecuteScalar());

                            requête = "SELECT pubid FROM publishers WHERE name = '" + editeur + "'";
                            var maCde2 = new NpgsqlCommand(requête, maCnx);
                            Int32 pubid = Convert.ToInt32(maCde2.ExecuteScalar());

                            requête = "INSERT INTO titles(title, isbn, pubid) VALUES('" + titre +"', '" + isbn +"', '" + pubid + "'); INSERT INTO titleauthor(isbn, au_id) VALUES('" + isbn + "', '" + auid + "')";              
                            var maCde3 = new NpgsqlCommand(requête, maCnx);
                            maCde3.ExecuteNonQuery();

                        }
                        catch (NpgsqlException e)
                        {
                            Console.WriteLine("Erreur " + e.ToString());
                        }
                        finally
                        {
                            if (maCnx is object & maCnx.State == ConnectionState.Open)
                            {
                                maCnx.Close(); // on se déconnecte
                            }
                        }
                        break;
                    case 6:
                        try
                        {
                        maCnx.Open();
                        requête = "SELECT p.pubid, p.name FROM publishers p";
                        var maCde = new NpgsqlCommand(requête, maCnx);
                        jeuEnr = maCde.ExecuteReader();
                            while (jeuEnr.Read())
                            {
                                Console.WriteLine("\n\nNom de l'éditeur : " + jeuEnr["name"] + "\n\n");
                                maCnx2.Open();
                                requête2 = "SELECT title from titles WHERE pubid = " + jeuEnr["pubid"];
                                var maCde2 = new NpgsqlCommand(requête2, maCnx2);
                                jeuEnr2 = maCde2.ExecuteReader();
                                while (jeuEnr2.Read())
                                {
                                    Console.WriteLine(jeuEnr2["title"]);
                                }
                                maCnx2.Close();
                            }
                        }


                        catch (NpgsqlException e)
                        {
                            Console.WriteLine("Erreur " + e.ToString());
                        }
                        finally
                        {
                            if (jeuEnr is object & !jeuEnr.IsClosed)
                            {
                                jeuEnr.Close(); // s'il existe et n'est pas déjà fermé
                            }
                            if (maCnx is object & maCnx.State == ConnectionState.Open)
                            {
                                maCnx.Close(); // on se déconnecte
                            }
                        }
                        break;
                }

            } while (choix <= 0 | choix >= 9);
            Console.ReadLine();
        }
    }
}
