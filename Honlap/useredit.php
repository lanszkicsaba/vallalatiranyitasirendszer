<html>
    <body>
        <?php
        include 'dbconnect.php';
        session_start();
        ?>
        <?php
        if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
            if (isset($_POST['submit'])) {
                $con = MySQLServerConnecter();
                mysqli_set_charset($con, "utf8");
                $result;
                $loginname = $_SESSION["user"];
                $pwd = md5($_POST['pw2']);
                if ($stmt = $con->prepare("UPDATE honlapusers SET password=?, fullname=?,address=?,phonenumer=? WHERE username=?")) {
                    $stmt->bind_param("sssss", $pwd, $_POST['fullname'], $_POST['lakcim'], $_POST['telszam'], $loginname
                    );
                    $stmt->execute();
                    $result = $stmt->get_result();
                    $stmt->close();
                }
            }

            if (isset($result)) {
                echo "<p align=\"center\" style=\"color:green;\">Az adatok sikeresen frissítve.</p>";
            }
            
        } else {
            echo '<meta http-equiv="refresh" content="0; URL=index.php">';
        }
        ?>
        <script>
            function check() {
                var hibauzenet = "";
                var hiba = false;
                var htmltags = /<(.|\n)*?>/g;
                var teljesnev = document.getElementById("fullname").value;
                var lakcim = document.getElementById("lakcim").value;
                var pw1 = document.getElementById("pw1").value;
                var pw2 = document.getElementById("pw2").value;
                var telszam = document.getElementById("telszam").value;
                if (teljesnev.search(" ") < 0 || teljesnev.match(htmltags))
                {
                    hibauzenet = hibauzenet + "Kérlek teljes nevet adj meg.\n";
                    hiba = true;
                }
                if (lakcim.search(" ") < 0 || lakcim.match(htmltags))
                {
                    hibauzenet = hibauzenet + "Kérlek teljes címet adj meg.\n";
                    hiba = true;
                }

                if (pw1.length > 0 || pw2.length > 0)
                {
                    if (pw1.length < 5 || pw2.length < 5 || pw1 != pw2)
                    {
                        hibauzenet = hibauzenet + "Túl rövid a jelszó, vagy nem egyeznek.\n";
                        hiba = true;

                    }
                }
                if (telszam.length < 9 || telszam.search("06") < 0)
                {
                    hibauzenet = hibauzenet + "Kérlek valós telefonszámot adj meg.\n";
                    hiba = true;

                }
                if (hiba == true)
                {
                    alert(hibauzenet);
                    return false;
                }
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
                    if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
                        $con = MySQLServerConnecter();
                        mysqli_set_charset($con, "utf8");

                        $loginname = $_SESSION["user"];
                        if ($stmt = $con->prepare("SELECT * FROM honlapusers WHERE username=?")) {
                            $stmt->bind_param("s", $loginname);
                            $stmt->execute();
                            $stmt->bind_result($id, $username, $password, $fullname, $address, $phonenumber);
                            $stmt->fetch();
                            $stmt->close();

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
				</tr>';
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