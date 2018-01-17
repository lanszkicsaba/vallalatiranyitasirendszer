<?php
include 'dbconnect.php';
session_start();
?>
<!DOCTYPE html>
<html>
	<head>
		<meta charset="UTF-8">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
		<title>Szűrtlista</title>
	</head>
<body>
<div class="container">
	<div class="page-header">
		<h1>Szűrtlista</h1>
	</div>
<?php
//vizsgáljuk hogy a felhasználó be van e jelentkezve
if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
	//ha be van jelentkezve
	//lekérjük a legnagyobb árat az adatbázisból
	$conn = MySQLServerConnecter();
	mysqli_set_charset($conn, "utf8");
	$select = "SELECT max(ar) as max_ar FROM termekek";
	$s = $conn->query($select);
	$conn->close();
	while ($row = $s->fetch_assoc()) {
		//elmentjük a legnagyobb árat egy globális báltozóba
		$_SESSION["maximum_ar"] = $row["max_ar"];
	}
	//ha nem adott volna meg értéket a minimum árnak
    if ($_POST['f_armin'] == "") {
        $_POST['f_armin'] = 1;
    }
    //ha nem adott volna meg értéket a maximum árnak, vagy az az alapártelmezett 1
	if ($_POST['f_armax'] == "" || $_POST['f_armax'] == 1) {
		//átadjuk a kakpott változónak a maximum árat
		$_POST['f_armax'] = $_SESSION["maximum_ar"];
    }
	//ha a felhasználó minden módosítás nélkül kattint a szűrés gombra
    if ($_POST['f_nev'] == "" && $_POST['f_armin'] == 1 && $_POST['f_armax'] == $_SESSION['maximum_ar']) {
        $conn = MySQLServerConnecter();
        mysqli_set_charset($conn, "utf8");
		//lekérdezzük az összes terméket
        $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek";
        $s = $conn->query($select);
        $conn->close();
		//a lekért termékeket táblába helyezzük
        echo "<table class='table table-striped'>";
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
				  <input type='hidden' name=id value='" . $row["id"] . "'>
				  <button type='submit' class='btn btn-cart'>Kosárba</button>"
            . "</tr></form>";
        }
        echo "</table>";
    }
    else {
		//ha a felhasználó csak név alapján szűri a termékeket
        if ($_POST['f_nev'] <> "" && $_POST['f_armin'] == 1 && $_POST['f_armax'] == $_SESSION['maximum_ar']) {
            $conn = MySQLServerConnecter();
            mysqli_set_charset($conn, "utf8");
			//lekérdezzük a felhasználó által megadott nevű termékeket
            $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek WHERE termeknev like '%$_POST[f_nev]%'";
            $s = $conn->query($select);
            $conn->close();
			//viszgáljuk hogy van e ilyen termék
            if ($s->num_rows > 0) {
				//ha van akkor feltöltjük a táblázatot
                echo "<table class='table table-striped'>";
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
				  <input type='hidden' name=id value='" . $row["id"] . "'>
				  <button type='submit' class='btn btn-cart'>Kosárba</button>"
                    . "</tr></form>";
                }
                echo "</table>";
            }
            else {
				//ha nincs akkor kiírjuk hogy nincs megfelelő elem
                echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
            }
        } else {
			//ha a felhasználó nem tölti ki a név szerinti szűrést
            if ($_POST['f_nev'] == "") {
				
                $conn = MySQLServerConnecter();
                mysqli_set_charset($conn, "utf8");
				//lekérdezünk minden terméket, amely a minimum és maximum intervallumba esik
                $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek WHERE ar >= " . $_POST['f_armin'] . " and ar <= " . $_POST['f_armax'] . ";";
                $s = $conn->query($select);
                $conn->close();
				//ha van ilyen termék
                if ($s->num_rows > 0) {
					//feltöltjük a táblába
                    echo "<table class='table table-striped'>";
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
				  <input type='hidden' name=id value='" . $row["id"] . "'>
				  <button type='submit' class='btn btn-cart'>Kosárba</button>"
                        . "</tr></form>";
                    }
                    echo "</table>";
                }
                else {
					//ha nincs ilyen termék akkor kiírjuk hogy nincs megfelelő elem
                    echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
                }
            } else {
				//ha a felhasználó minden mezőt kitölt
                if ($_POST['f_nev'] <> "") {
                    $conn = MySQLServerConnecter();
                    mysqli_set_charset($conn, "utf8");
                    $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek WHERE termeknev like '%$_POST[f_nev]%' and ar >= " . $_POST['f_armin'] . " and ar <= " . $_POST['f_armax'] . ";";
                    $s = $conn->query($select);
                    $conn->close();
                    if ($s->num_rows > 0) {
						//ha van a szűrésnek megfelelő elem feltöltjük a táblába
                        echo "<table class='table table-striped'>";
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
				  <input type='hidden' name=id value='" . $row["id"] . "'>
				  <button type='submit' class='btn btn-cart'>Kosárba</button>"
                            . "</tr></form>";
                        }
                        echo "</table>";
                    }
                    else {
						//ha nincs akkor Nincs a szűrésnek megfelelő elem üzenetet adunk.
                        echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
                    }
                }
            }
        }
    }
} else {
//ha nincs belépve
	$conn = MySQLServerConnecter();
	mysqli_set_charset($conn, "utf8");
	$select = "SELECT max(ar) as max_ar FROM termekek";
	$s = $conn->query($select);
	$conn->close();
	while ($row = $s->fetch_assoc()) {
		$_SESSION["maximum_ar"] = $row["max_ar"];
	}
	//ha nem adott volna meg értéket a minimum árnak
    if ($_POST['f_armin'] == "") {
        $_POST['f_armin'] = 1;
    }
	//ha nincs maximum ár, vagy az az alapértelmezett 1
    if ($_POST['f_armax'] == "" || $_POST['f_armax'] == 1) {
		//elentjük a globális változó értékét
		$_POST['f_armax'] = $_SESSION["maximum_ar"];
    }
	//ha minden kitöltés nélkül kattint a szűrés gombra
    if ($_POST['f_nev'] == "" && $_POST['f_armin'] == 1 && $_POST['f_armax'] == $_SESSION['maximum_ar']) {
        $conn = MySQLServerConnecter();
        mysqli_set_charset($conn, "utf8");
		//lekérdezzük az összes terméket
        $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek";
        $s = $conn->query($select);
        $conn->close();
		//elmentjük a táblába kosárba rakás gomb nélkül
        echo "<table class='table table-striped'>";
        echo "<tr>"
        . "<th>Fénykép</th>"
        . "<th>Terméknév</th>"
        . "<th>Leírás</th>"
        . "<th>Készleten</th>"
        . "<th>Ár</th>"
        . "</tr>";
        while ($row = $s->fetch_assoc()) {
            echo "<tr>"
            . "<td><img src=./image/" . $row["kep"] . " alt=fenykep style=width:128px;height:128px;></td>"
            . "<td>" . $row["termeknev"] . "</td>"
            . "<td>" . $row["leiras"] . "</td>";
            if ($row["keszleten"] == 0)
                echo "<td>Nincs készleten</td>";
            else
                echo "<td>Van készleten</td>";
            echo "<td>" . $row["ar"] . " Ft</td>"
            . "</tr>";
        }
        echo "</table>";
    }
    else {
		//ha csak a név mezőt tölti ki
        if ($_POST['f_nev'] <> "" && $_POST['f_armin'] == 1 && $_POST['f_armax'] == $_SESSION['maximum_ar']) {
            $conn = MySQLServerConnecter();
            mysqli_set_charset($conn, "utf8");
            $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek WHERE termeknev like '%$_POST[f_nev]%'";
            $s = $conn->query($select);
            $conn->close();
			//vizsgáljuk hogy van e ilyen elem
            if ($s->num_rows > 0) {
				//ha van akkor kitöltjük  a táblát
                echo "<table class='table table-striped'>";
                echo "<tr>"
                . "<th>Fénykép</th>"
                . "<th>Terméknév</th>"
                . "<th>Leírás</th>"
                . "<th>Készleten</th>"
                . "<th>Ár</th>"
                . "</tr>";
                while ($row = $s->fetch_assoc()) {
                    echo "<tr>"
                    . "<td><img src=./image/" . $row["kep"] . " alt=fenykep style=width:128px;height:128px;></td>"
                    . "<td>" . $row["termeknev"] . "</td>"
                    . "<td>" . $row["leiras"] . "</td>";
                    if ($row["keszleten"] == 0)
                        echo "<td>Nincs készleten</td>";
                    else
                        echo "<td>Van készleten</td>";
                    echo "<td>" . $row["ar"] . " Ft</td>"
                    . "</tr>";
                }
                echo "</table>";
            }
            else {
				//ha nincs akkor üzenetet írunk ki
                echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
            }
        } else {
			//ha nem tölti ki a név mezőt
            if ($_POST['f_nev'] == "") {
                $conn = MySQLServerConnecter();
                mysqli_set_charset($conn, "utf8");
				//lekérjük az ár intervallumba eső termékeket
                $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek WHERE ar >= " . $_POST['f_armin'] . " and ar <= " . $_POST['f_armax'] . ";";
                $s = $conn->query($select);
                $conn->close();
				//vizsgáljuk hogy van e ilyen termék
                if ($s->num_rows > 0) {
					//ha van akkor feltöltjük vele a táblát
                    echo "<table class='table table-striped'>";
                    echo "<tr>"
                    . "<th>Fénykép</th>"
                    . "<th>Terméknév</th>"
                    . "<th>Leírás</th>"
                    . "<th>Készleten</th>"
                    . "<th>Ár</th>"
                    . "</tr>";
                    while ($row = $s->fetch_assoc()) {
                        echo "<tr>"
                        . "<td><img src=./image/" . $row["kep"] . " alt=fenykep style=width:128px;height:128px;></td>"
                        . "<td>" . $row["termeknev"] . "</td>"
                        . "<td>" . $row["leiras"] . "</td>";
                        if ($row["keszleten"] == 0)
                            echo "<td>Nincs készleten</td>";
                        else
                            echo "<td>Van készleten</td>";
                        echo "<td>" . $row["ar"] . " Ft</td>"
                        . "</tr>";
                    }
                    echo "</table>";
                }
                else {
					//ha nincs akkor üzenetet írunk ki 
                    echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
                }
            } else {
				//a felhasználó minden mezőt kitölt
                if ($_POST['f_nev'] <> "") {
                    $conn = MySQLServerConnecter();
                    mysqli_set_charset($conn, "utf8");
					//lekérdezzük a szűrésnek megfelelő termékeket
                    $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek WHERE termeknev like '%$_POST[f_nev]%' and ar >= " . $_POST['f_armin'] . " and ar <= " . $_POST['f_armax'] . ";";
                    $s = $conn->query($select);
                    $conn->close();
                    if ($s->num_rows > 0) {
						//ha van ilyen akkor fetöltjük a táblát
                        echo "<table class='table table-striped'>";
                        echo "<tr>"
                        . "<th>Fénykép</th>"
                        . "<th>Terméknév</th>"
                        . "<th>Leírás</th>"
                        . "<th>Készleten</th>"
                        . "<th>Ár</th>"
                        . "</tr>";
                        while ($row = $s->fetch_assoc()) {
                            echo "<tr>"
                            . "<td><img src=./image/" . $row["kep"] . " alt=fenykep style=width:128px;height:128px;></td>"
                            . "<td>" . $row["termeknev"] . "</td>"
                            . "<td>" . $row["leiras"] . "</td>";
                            if ($row["keszleten"] == 0)
                                echo "<td>Nincs készleten</td>";
                            else
                                echo "<td>Van készleten</td>";
                            echo "<td>" . $row["ar"] . " Ft</td>"
                            . "</tr>";
                        }
                        echo "</table>";
                    }
                    else {
						//ha nincs akkor üzenetet írunk ki
                        echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
                    }
                }
            }
        }
    }
}
//vissza gomb az index.php-ra
echo "<html>";
echo '<form action="index.php" method="post">
	<input class="btn btn-default" type="submit" value="Vissza">
	</form>';
echo "</html>";
?>
</div>
</body>
</html>