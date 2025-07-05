using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace SMFileReadingTest
{
    class Program
    {
        /* This is a console app that finds all the .sm files in a folder and its subdirectories,
         * loops through them, puts any available information about them into a song object, and
         * prints the results. This does not go over every possible property that a .sm file could have
         * because this is a test and I think I would drop dead if I had to type all of that.
         * I might expand it into a way to edit .sm file properties without using a text editor.
         * Even though there are already ways to do that.
         * lol
         * There has got to be a better way to write all this but this is an amateur project written by
         * someone who hasn't touched C# in a long time.
         */
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path containing your Stepmania songs: ");
            string smPath = Console.ReadLine();
            Console.WriteLine($"Searching {smPath}...");

            try
            {
                List<string> paths = Directory.GetFiles(smPath, "*.sm", SearchOption.AllDirectories).ToList();
                Console.WriteLine($"Found {paths.Count} songs. Press any key to continue.");
                Console.ReadKey();
                List<Songs> songsList = new List<Songs>();

                foreach (string path in paths)
                {
                    Dictionary<string, string> songInfo = new Dictionary<string, string>();
                    try
                    {

                        var lines = File.ReadLines(path)
                            .TakeWhile(line => !string.IsNullOrEmpty(line) && line.StartsWith("#"));

                        // Clean up the song information, put them in songInfo dict
                        foreach (var line in lines)
                        {
                            var split = line.Split(':');
                            string key = split[0].Trim('#');
                            string value = split[1].Trim(';');
                            songInfo.Add(key, value);
                        }

                        Console.WriteLine("\nCreating Songs object...");

                        Songs songs = new Songs
                        {
                            // I'm going to lose my mind

                            Title = songInfo.GetValueOrDefault("TITLE", " "),
                            Subtitle = songInfo.GetValueOrDefault("SUBTITLE", " "),
                            Artist = songInfo.GetValueOrDefault("ARTIST", " "),
                            TitleTransliteration = songInfo.GetValueOrDefault("TITLETRANSLIT", " "),
                            ArtistTransliteration = songInfo.GetValueOrDefault("ARTISTTRANSLIT", " "),
                            SubtitleTransliteration = songInfo.GetValueOrDefault("SUBTITLETRANSLIT", " "),
                            Genre = songInfo.GetValueOrDefault("GENRE", " "),
                            Credit = songInfo.GetValueOrDefault("CREDIT", " "),
                            BannerPath = songInfo.GetValueOrDefault("BANNER", " "),
                            BackgroundPath = songInfo.GetValueOrDefault("BACKGROUND", " "),
                            LyricsPath = songInfo.GetValueOrDefault("LYRICS", " "),
                            CDTitlePath = songInfo.GetValueOrDefault("CDTITLE", " "),
                            MusicPath = songInfo.GetValueOrDefault("MUSIC", " "),
                            BPMs = songInfo.GetValueOrDefault("BPM", " "),
                            Selectable = songInfo.GetValueOrDefault("SELECTABLE", " "),
                            Stops = songInfo.GetValueOrDefault("STOPS", " "),
                            Freezes = songInfo.GetValueOrDefault("FREEZES", " "),
                            Delays = songInfo.GetValueOrDefault("DELAYS", " "),
                            BGChanges = songInfo.GetValueOrDefault("BGCHANGES", " "),
                            Keysounds = songInfo.GetValueOrDefault("KEYSOUNDS", " "),
                            Attacks = songInfo.GetValueOrDefault("ATTACKS", " "),
                            DisplayBPM = songInfo.GetValueOrDefault("DISPLAYBPM", " ") 
                            //There has got to be a better way to do this
                            
                        };

                        if (songInfo.TryGetValue("VERSION", out var versionString))
                            if (double.TryParse(versionString, out var version))
                                songs.Version = version;
                        if (songInfo.TryGetValue("OFFSET", out var offsetString))
                            if (double.TryParse(offsetString, out var offset))
                                songs.Offset = offset;
                        if (songInfo.TryGetValue("SAMPLESTART", out var sampleStartString))
                            if (double.TryParse(sampleStartString, out var sampleStart))
                                songs.SampleStart = sampleStart;
                        if (songInfo.TryGetValue("SAMPLELENGTH", out var sampleLengthString))
                            if (double.TryParse(sampleLengthString, out var sampleLength))
                                songs.SampleLength = sampleLength;
                        Console.WriteLine($"DEBUG: Adding new song to songs list: {songs.Title} by {songs.Artist}");
                        songsList.Add(songs);

                        Console.WriteLine("Songs details:");
                        Console.WriteLine(songs?.ToString() ?? "Songs is null");
                        Console.WriteLine("--------------------------------");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing {Path.GetFileName(path)}:");
                        Console.WriteLine(ex);
                        Console.WriteLine("--------------------------------");
                    }
                }
                string connectionString = @"Data Source=C:\Test files\song_db.db";

                try
                {
                    // Create a table if it does not exist already
                    String SQLQueryCreateTable = @"CREATE TABLE IF NOT EXISTS songs(id INTEGER PRIMARY KEY,
                                                                         title VARCHAR NOT NULL,
                                                                         artist VARCHAR DEFAULT 'Artist unknown',
                                                                         subtitle VARCHAR DEFAULT 'No Subtitle',
                                                                         title_transliteration VARCHAR DEFAULT 'N/A',
                                                                         artist_transliteration VARCHAR DEFAULT 'N/A',
                                                                         subtitle_transliteration VARCHAR DEFAULT 'N/A',
                                                                         genre VARCHAR DEFAULT 'N/A',
                                                                         banner_path VARCHAR DEFAULT 'N/A',
                                                                         background_path VARCHAR DEFAULT 'N/A',
                                                                         lyrics_path VARCHAR DEFAULT 'N/A',
                                                                         cdtitle_path VARCHAR DEFAULT 'N/A',
                                                                         music_path VARCHAR DEFAULT 'N/A',
                                                                         bpms VARCHAR DEFAULT 'N/A',
                                                                         version REAL DEFAULT NULL,
                                                                         song_offset REAL DEFAULT 0,
                                                                         sample_start REAL DEFAULT 0,
                                                                         sample_length REAL DEFAULT 0,
                                                                         selectable VARCHAR DEFAULT 'N/A',
                                                                         stops VARCHAR DEFAULT 'N/A',
                                                                         freezes VARCHAR DEFAULT 'N/A',
                                                                         delays VARCHAR DEFAULT 'N/A',
                                                                         bg_changes VARCHAR DEFAULT 'N/A',
                                                                         keysounds VARCHAR DEFAULT 'N/A',
                                                                         attacks VARCHAR DEFAULT 'N/A',
                                                                         display_bpm VARCHAR DEFAULT 'N/A')";

                    // Insert required data into table
                    string SQLQueryInsertData = @"INSERT INTO songs(title,
                                                                    artist,
                                                                    subtitle,
                                                                    title_transliteration,
                                                                    artist_transliteration,
                                                                    subtitle_transliteration,
                                                                    genre,
                                                                    banner_path,
                                                                    background_path,
                                                                    lyrics_path,
                                                                    cdtitle_path,
                                                                    music_path,
                                                                    bpms,
                                                                    version,
                                                                    song_offset,
                                                                    sample_start,
                                                                    sample_length,
                                                                    selectable,
                                                                    stops,
                                                                    freezes,
                                                                    delays,
                                                                    bg_changes,
                                                                    keysounds,
                                                                    attacks,
                                                                    display_bpm) VALUES (@titleParam, 
                                                                                        @artistParam,
                                                                                        @subtitleParam,
                                                                                        @titleTransliterationParam,
                                                                                        @artistTransliterationParam,
                                                                                        @subtitleTransliterationParam,
                                                                                        @genreParam,
                                                                                        @bannerPathParam,
                                                                                        @backgroundPathParam,
                                                                                        @lyricsPathParam,
                                                                                        @cdtitlePathParam,
                                                                                        @musicPathParam,
                                                                                        @bpmsParam,
                                                                                        @versionParam,
                                                                                        @songOffsetParam,
                                                                                        @sampleStartParam,
                                                                                        @sampleLengthParam,
                                                                                        @selectableParam,
                                                                                        @stopsParam,
                                                                                        @freezesParam,
                                                                                        @delaysParam,
                                                                                        @bgChangesParam,
                                                                                        @keysoundsParam,
                                                                                        @attacksParam,
                                                                                        @displayBPMParam)";

                    // Connect to or create new SQLite database file
                    using var connection = new SqliteConnection(connectionString);
                    connection.Open();
                    Console.WriteLine("Connection to database successful");

                    // SQLite command that makes a new table
                    SqliteCommand SQLCommandCreateTable = new SqliteCommand(SQLQueryCreateTable, connection);
                    // SQLite command that inserts data
                    SqliteCommand SQLCommandInsertData = new SqliteCommand(SQLQueryInsertData, connection);

                    SQLCommandCreateTable.ExecuteNonQuery();
                    
                    string SQLQueryCheckExisting = @"SELECT COUNT(*) 
                                                     FROM songs 
                                                     WHERE title = @title 
                                                     AND artist = @artist 
                                                     AND music_path = @musicPath";
                    
                    var SQLCommandCheckExisting = new SqliteCommand(SQLQueryCheckExisting, connection);

                    foreach (var song in songsList)
                    {
                        try
                        {
                            var songTitle = song.Title;
                            var songArtist = song.Artist;
                            SQLCommandCheckExisting.Parameters.Clear();
                            SQLCommandCheckExisting.Parameters.AddWithValue("@title", song.Title);
                            SQLCommandCheckExisting.Parameters.AddWithValue("@artist", song.Artist);
                            SQLCommandCheckExisting.Parameters.AddWithValue("@musicPath",
                                string.IsNullOrWhiteSpace(song.MusicPath) ? DBNull.Value : song.MusicPath);

                            long alreadyExists = (long)SQLCommandCheckExisting.ExecuteScalar();

                            if (alreadyExists == 0)
                            {
                                SQLCommandInsertData.Parameters.Clear();
                                
                                // Insert title, artist
                                SQLCommandInsertData.Parameters.AddWithValue("@titleParam", songTitle);
                                SQLCommandInsertData.Parameters.AddWithValue("@artistParam", songArtist);

                                // Insert song information
                                SQLCommandInsertData.Parameters.AddWithValue("@subtitleParam",
                                    string.IsNullOrWhiteSpace(song.Subtitle) ? DBNull.Value : song.Subtitle);
                                SQLCommandInsertData.Parameters.AddWithValue("@titleTransliterationParam",
                                    string.IsNullOrWhiteSpace(song.TitleTransliteration)
                                        ? DBNull.Value
                                        : song.TitleTransliteration);
                                SQLCommandInsertData.Parameters.AddWithValue("@artistTransliterationParam",
                                    string.IsNullOrWhiteSpace(song.ArtistTransliteration)
                                        ? DBNull.Value
                                        : song.ArtistTransliteration);
                                SQLCommandInsertData.Parameters.AddWithValue("@subtitleTransliterationParam",
                                    string.IsNullOrWhiteSpace(song.SubtitleTransliteration)
                                        ? DBNull.Value
                                        : song.SubtitleTransliteration);
                                SQLCommandInsertData.Parameters.AddWithValue("@genreParam",
                                    string.IsNullOrWhiteSpace(song.Genre) ? DBNull.Value : song.Genre);
                                SQLCommandInsertData.Parameters.AddWithValue("@bannerPathParam",
                                    string.IsNullOrWhiteSpace(song.BannerPath) ? DBNull.Value : song.BannerPath);
                                SQLCommandInsertData.Parameters.AddWithValue("@backgroundPathParam",
                                    string.IsNullOrWhiteSpace(song.BackgroundPath)
                                        ? DBNull.Value
                                        : song.BackgroundPath);
                                SQLCommandInsertData.Parameters.AddWithValue("@lyricsPathParam",
                                    string.IsNullOrWhiteSpace(song.LyricsPath) ? DBNull.Value : song.LyricsPath);
                                SQLCommandInsertData.Parameters.AddWithValue("@cdtitlePathParam",
                                    string.IsNullOrWhiteSpace(song.CDTitlePath) ? DBNull.Value : song.CDTitlePath);
                                SQLCommandInsertData.Parameters.AddWithValue("@musicPathParam",
                                    string.IsNullOrWhiteSpace(song.MusicPath) ? DBNull.Value : song.MusicPath);
                                SQLCommandInsertData.Parameters.AddWithValue("@bpmsParam",
                                    string.IsNullOrWhiteSpace(song.BPMs) ? DBNull.Value : song.BPMs);

                                // Insert numeric song details
                                SQLCommandInsertData.Parameters.AddWithValue("@versionParam",
                                    song.Version.HasValue ? song.Version.Value : DBNull.Value);
                                SQLCommandInsertData.Parameters.AddWithValue("@songOffsetParam",
                                    song.Offset.HasValue ? song.Offset.Value : DBNull.Value);
                                SQLCommandInsertData.Parameters.AddWithValue("@sampleStartParam",
                                    song.SampleStart.HasValue ? song.SampleStart.Value : DBNull.Value);
                                SQLCommandInsertData.Parameters.AddWithValue("@sampleLengthParam",
                                    song.SampleLength.HasValue ? song.SampleLength.Value : DBNull.Value);

                                // Insert game-related details
                                SQLCommandInsertData.Parameters.AddWithValue("@selectableParam",
                                    string.IsNullOrWhiteSpace(song.Selectable) ? DBNull.Value : song.Selectable);
                                SQLCommandInsertData.Parameters.AddWithValue("@stopsParam",
                                    string.IsNullOrWhiteSpace(song.Stops) ? DBNull.Value : song.Stops);
                                SQLCommandInsertData.Parameters.AddWithValue("@freezesParam",
                                    string.IsNullOrWhiteSpace(song.Freezes) ? DBNull.Value : song.Freezes);
                                SQLCommandInsertData.Parameters.AddWithValue("@delaysParam",
                                    string.IsNullOrWhiteSpace(song.Delays) ? DBNull.Value : song.Delays);
                                SQLCommandInsertData.Parameters.AddWithValue("@bgChangesParam",
                                    string.IsNullOrWhiteSpace(song.BGChanges) ? DBNull.Value : song.BGChanges);
                                SQLCommandInsertData.Parameters.AddWithValue("@keysoundsParam",
                                    string.IsNullOrWhiteSpace(song.Keysounds) ? DBNull.Value : song.Keysounds);
                                SQLCommandInsertData.Parameters.AddWithValue("@attacksParam",
                                    string.IsNullOrWhiteSpace(song.Attacks) ? DBNull.Value : song.Attacks);
                                SQLCommandInsertData.Parameters.AddWithValue("@displayBPMParam",
                                    string.IsNullOrWhiteSpace(song.DisplayBPM) ? DBNull.Value : song.DisplayBPM);

                                Console.WriteLine($"Inserting {songTitle} by {songArtist}");
                                var insertResult = SQLCommandInsertData.ExecuteNonQuery();
                                Console.WriteLine($"Insert result: {insertResult}");
                                Console.WriteLine("--------------------------------");
                            }
                            else
                            {
                                Console.WriteLine($"Skipping duplicate file: {songTitle} by {songArtist}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error adding file: {song.Title} by {song.Artist}:");
                            Console.WriteLine($"Error message: {ex.Message}");
                            Console.WriteLine($"Stack trace: {ex.StackTrace}");
                            continue;

                        }
                    }

                    SQLCommandInsertData.ExecuteNonQueryAsync();
                    
                    connection.Close();
                    Console.WriteLine("Successfully updated database. Connection closed.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}