<html>
    <body>
        <?php
        include 'dbconnect.php';
        session_start();
        ?>
        <?php
        //ha bevan lépve
        if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
            if (isset($_POST['submit'])) { //Ha megnyomta a Módosítás gombot
                $con = MySQLServerConnecter(); //MySQL csatlakozás
                mysqli_set_charset($con, "utf8"); //Kódolás beállítása
                $result; //Lekérdezés eredménye
                $loginname = $_SESSION["user"]; //Bejelentkezett felhasználónév az adatok módosításához.
                $pwd = md5($_POST['pw2']); //MD5 kódolása a jelszónak
                if ($stmt = $con->prepare("UPDATE honlapusers SET password=?, fullname=?,address=?,phonenumer=?,Taxnumber=? WHERE username=?")) { //MYSQL statement létrehozása
                    $stmt->bind_param("ssssss", $pwd, $_POST['fullname'], $_POST['lakcim'], $_POST['telszam'], $_POST['adoszam'], $loginname //Változók hozzárendelése a statementhez
                    );
                    $stmt->execute(); //Statement futtatása
                    $result = $stmt->get_result(); //Lekérdezés eredményének hozzárendelése a $resulthoz
                    $stmt->close(); //Csatlakozás lezárása
                }
            }

            if (isset($result))  //Ha sikeresen végrehajtódott a query, akkor eredmény kiíratása
			{ 
                echo "<p align=\"center\" style=\"color:green;\">Az adatok sikeresen frissítve.</p>";
            }
            
        } 
		else
		{
            // Ha nincs bejelentkezve visszadobja az index.php-re
            echo '<meta http-equiv="refresh" content="0; URL=index.php">';
        }
        ?>
        <script>
			//Funkció a bevitt adatok ellenőrzésére
            function check() {
				//Változók deklarálása
                var hibauzenet = ""; //Hibaüzenet szövege.
                var hiba = false; //Volt-e hibás adat.
                var htmltags = /<(.|\n)*?>/g; //HTML injection kiszűrésére
                var teljesnev = document.getElementById("fullname").value;
                var lakcim = document.getElementById("lakcim").value;
                var pw1 = document.getElementById("pw1").value;
                var pw2 = document.getElementById("pw2").value;
                var telszam = document.getElementById("telszam").value;
				var adoszam = document.getElementById("adoszam").value;
                if (teljesnev.search(" ") < 0 || teljesnev.match(htmltags)) //Ha nem áll legalább 2 szóból (space) vagy nem megengedett karakterek szerepelnek benne
                {
                    hibauzenet = hibauzenet + "Kérlek teljes nevet adj meg.\n";
                    hiba = true;
                }
                if (lakcim.search(" ") < 0 || lakcim.match(htmltags)) //Ha nem áll legalább 2 szóból (space) vagy nem megengedett karakterek szerepelnek benne
                {
                    hibauzenet = hibauzenet + "Kérlek teljes címet adj meg.\n";
                    hiba = true;
                }

                if (pw1.length > 0 || pw2.length > 0) //Ha változik a jelszó
                {
                    if (pw1.length < 5 || pw2.length < 5 || pw1 != pw2) //Minimum 5 karakter megvan-e és egyezik-e a két mező
                    {
                        hibauzenet = hibauzenet + "Túl rövid a jelszó, vagy nem egyeznek.\n";
                        hiba = true;

                    }
                }
                if (telszam.length < 9 || telszam.search("06") < 0) //Minimum 9 karakter megvan-e és tartalmazza-e a 06 előhívót
                {
                    hibauzenet = hibauzenet + "Kérlek valós telefonszámot adj meg.\n";
                    hiba = true;

                }
				
				if (adoszam.length!=11) //Megvan-e a 11 karakter hossz.
				{
					hibauzenet = hibauzenet + "Kérlek valós adószámot adj meg.\n";
					hiba=true;
				}
                if (hiba == true) //Ha volt hibás bevitel, akkor írja ki a hibaüzenetet és térjen vissza hamissan a függvény.
                {
                    alert(hibauzenet);
                    return false;
                }
				//Ha minden rendben volt, a visszatérés true.
                return true;
            }
        </script>
        <form method="post" action="" onsubmit="return check()" enctype="multipart/form-data">
            <table align="center">
                <tfoot>
                    <tr>
                        <td colspan="2" style="background:#99ccff;border-top: 2px solid #444444;">
                            <div style="text-align:center">
                                <input type="submit" name="submit" value="Módosítás"/>
                            </div>
                        </td>
                    </tr>
                </tfoot>
                <tbody>
                    <?php
                    if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") { //Ha a felh. bevan jelentkezve, akkor töltse be a módosítás formját.
                        $con = MySQLServerConnecter();
                        mysqli_set_charset($con, "utf8");

                        $loginname = $_SESSION["user"]; //Sessionból a bejelentkezett név a szűréshez
                        if ($stmt = $con->prepare("SELECT * FROM honlapusers WHERE username=?")) { //MySQL Statement létrehozása
                            $stmt->bind_param("s", $loginname); //Paraméterek hozzárendelése a statementhez
                            $stmt->execute(); //Statement futtatása
                            $stmt->bind_result($id, $username, $password, $fullname, $address, $phonenumber,$taxnumber); //A visszatérő adatokhoz szükséges változók hozzárendelése a statementhez.
                            $stmt->fetch(); //Adatok lekérése.
                            $stmt->close(); //Csatlakozás lezárása.
							//A tábla és a benne lévő mezők betöltése. A mezőkhöz a megfelelő értékek hozzárendelése.
                            echo '
				
				<tr>
					<td>Felhasználónév:</td>
					<td>
					<p id="nev" name="nev">' . $username . '</p>
					</td>
				</tr>
				
				<tr>
					<td>Új jelszó:
					<p style="font-size:75%">Minimum 5 karakter.</p></td>
					<td><input type="password" id="pw1" name="pw1"/></td>
				</tr>

				<tr>
					<td>Új jelszó ismét:</td>
					<td><input type="password" id="pw2" name="pw2"/></td>
				</tr>
				
				<tr>
					<td>Teljes név:</td>
					<td><input type="text" id="fullname" name="fullname" value="' . $fullname . '"/></td>
				</tr>
				
				<tr>
					<td>Lakcím:</td>
					<td><input type="text" id="lakcim" name="lakcim" value="' . $address . '"/></td>
				</tr>

				<tr>
					<td>Telefonszám:
					<p style="font-size:75%">Csak számok.</p></td>
					<td><input type="number" id="telszam" name="telszam" value="' . $phonenumber . '"/></td>
				</tr>
				<tr>
					<td>Adószám:</td>
					<td><input type="number" id="adoszam" min="0" name="adoszam" value="' . $taxnumber . '"/></td>
				</tr>
				'
				;
                        }
                    } else {
                        echo '<meta http-equiv="refresh" content="0; URL=index.php">';
                    }
                    ?>

                </tbody>
            </table>
            <br>
        </form>	
    <body>
</html>