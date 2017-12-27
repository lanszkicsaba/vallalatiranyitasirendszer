<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8" lang="hu">
        <title></title>
	<!--	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
		<script>
	<//?php include 'kosar.js';?>
	</script>-->
    </head>
	
    <body>
        <?php
        require_once 'dbconnect.php';
        session_start();
        ?>
		<script>
            function validateForm() {
                var armin = document.forms["szures"]["f_armin"].value;
                var armax = document.forms["szures"]["f_armax"].value;
                if (armin > armax) {
                    alert("A minimum ár nem lehet nagyobb a maximumnál!");
                    return false;
                }
            }
        </script>
        <?php
        $conn = MySQLServerConnecter();
        mysqli_set_charset($conn, "utf8");
        $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek";
        $s = $conn->query($select);
        $conn->close();

        if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
            echo "<p><a href=./logout.php>Kijelentkezés</a></p>";
            echo "<p><a href=./useredit.php>Adatok módosítása</a></p>";
			echo '<form name="szures" action="szurtlista.php" onsubmit="return validateForm()" method="post">
				Név:<br> <input type="text" name="f_nev"><br>
				Ár:<br> <input type="number" name="f_armin">-tól<br>
				<input type="number" name="f_armax">-ig<br>
				<br>
				<input type="submit" value="Szűrés"/>
				</form>';
        } else {
            echo "<p><a href=./login.php>Bejelentkezés</a></p>";
            echo "<p><a href=./regisztracio.php>Regisztráció</a></p>";
			echo '<form name="szures" action="szurtlista.php" onsubmit="return validateForm()" method="post">
				Név:<br> <input type="text" name="f_nev"><br>
				Ár:<br> <input type="number" name="f_armin">-tól<br>
				<input type="number" name="f_armax">-ig<br>
				<br>
				<input type="submit" value="Szűrés"/>
				</form>';
			}
            echo "<table>";
            echo "<tr>"
            . "<th>Fénykép</th>"
            . "<th>Terméknév</th>"
            . "<th>Leírás</th>"
            . "<th>Készleten</th>"
            . "<th>Ár</th>"
			. "<th>Kosár</th>"
            . "</tr>";
            while ($row = $s->fetch_assoc()) {
                echo "<form action=kosar.php method=post><tr>"
                . "<td><img src=./image/" . $row["kep"] . " alt=fenykep style=width:128px;height:128px;></td>"
                . "<td class='termeknev'>" . $row["termeknev"] . "</td>"
                . "<td>" . $row["leiras"] . "</td>";
                if ($row["keszleten"] == 0)
                    echo "<td>Nincs készleten</td>";
                else
                    echo "<td>Van készleten</td>";
                echo "<td class='termekar'>" . $row["ar"] . " Ft</td>"              
				. "<td>	
				  <input type='hidden' name=id value='".$row["id"]."'>
				  <button type='submit' class='btn btn-cart'>Kosárba</button>"
				. "</tr></form>";
            }
            echo "</table>";
        
        ?>
		
		
    </body>
</html>
