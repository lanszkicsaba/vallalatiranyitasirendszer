<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8" lang="hu">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
        <title></title>
	<!--	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
		<script>
	<//?php include 'kosar.js';?>
	</script>-->
	</head>
	
    <body>
	<div class="container">
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
		<div class="page-header">
		<h1>Webshop</h1>
		</div>
        <?php
        $conn = MySQLServerConnecter();
        mysqli_set_charset($conn, "utf8");
        $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek";
        $s = $conn->query($select);
        $conn->close();

        if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
		echo '<div class="list-group">';
		echo '<div class="row">';
		echo '<div class="col-sm-4">';
			echo "<a href=./kosar.php class='list-group-item active'>Kosár</a>";
		echo '</div>';
		echo '<div class="col-sm-4">';
            echo "<a href=./useredit.php class='list-group-item active'>Adatok módosítása</a>";
		echo '</div>';
		echo '<div class="col-sm-4">';
			echo "<a href=./logout.php class='list-group-item active'>Kijelentkezés</a>";
            
		echo '</div>';
		echo '</div>';
		echo '</div>';
		echo '<div class="row">';
		echo '<div class="col-sm-4">';
			echo '<form name="szures" action="szurtlista.php" onsubmit="return validateForm()" method="post">
				Név:<br> <input type="text" name="f_nev"><br>
				Ár:<br> <input type="number" name="f_armin">-tól<br>
				<input type="number" name="f_armax">-ig<br>
				<br>
				<input type="submit" value="Szűrés"/>
				</form>';
        echo '</div>';
		echo '<div class="col-sm-8">';
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
			echo '</div>';
		
            
        } else {
		echo '<div class="list-group">';
		echo '<div class="row">';
		echo '<div class="col-sm-6">';
            echo "<a href=./login.php class='list-group-item active'>Bejelentkezés</a>";
		echo '</div>';
		echo '<div class="col-sm-6">';
            echo "<a href=./regisztracio.php class='list-group-item active'>Regisztráció</a>";
		echo '</div>';
		echo '</div>';
		echo '</div>';
		echo '<div class="row">';
		echo '<div class="col-sm-4">';
			echo '<form name="szures" action="szurtlista.php" onsubmit="return validateForm()" method="post">
				Név:<br> <input type="text" name="f_nev"><br>
				Ár:<br> <input type="number" name="f_armin">-tól<br>
				<input type="number" name="f_armax">-ig<br>
				<br>
				<input type="submit" value="Szűrés"/>
				</form>';
		echo '</div>';
		echo '<div class="col-sm-8">';	
            echo "<table>";
            echo "<tr>"
            . "<th>Fénykép</th>"
            . "<th>Terméknév</th>"
            . "<th>Leírás</th>"
            . "<th>Készleten</th>"
            . "<th>Ár</>"
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
                echo "<td class='termekar'>" . $row["ar"] . " Ft</td>";
            }
            echo "</table>";
		echo '</div>';
        }
        ?>
	</div>
    </body>
</html>
