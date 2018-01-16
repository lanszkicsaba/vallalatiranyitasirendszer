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

if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
	$conn = MySQLServerConnecter();
	mysqli_set_charset($conn, "utf8");
	$select = "SELECT max(ar) as max_ar FROM termekek";
	$s = $conn->query($select);
	$conn->close();
	while ($row = $s->fetch_assoc()) {
		$_SESSION["maximum_ar"] = $row["max_ar"];
	}
//ha bevan lépve
    if ($_POST['f_armin'] == "") {
        $_POST['f_armin'] = 1;
    }
        if ($_POST['f_armax'] == "" || $_POST['f_armax'] == 1) {
		$_POST['f_armax'] = $_SESSION["maximum_ar"];
    }
    if ($_POST['f_nev'] == "" && $_POST['f_armin'] == 1 && $_POST['f_armax'] == $_SESSION['maximum_ar']) {
        $conn = MySQLServerConnecter();
        mysqli_set_charset($conn, "utf8");
        $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek";
        $s = $conn->query($select);
        $conn->close();
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
        if ($_POST['f_nev'] <> "" && $_POST['f_armin'] == 1 && $_POST['f_armax'] == $_SESSION['maximum_ar']) {
            $conn = MySQLServerConnecter();
            mysqli_set_charset($conn, "utf8");
            $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek WHERE termeknev like '%$_POST[f_nev]%'";
            $s = $conn->query($select);
            $conn->close();
            if ($s->num_rows > 0) {
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
                echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
            }
        } else {
            if ($_POST['f_nev'] == "") {
                $conn = MySQLServerConnecter();
                mysqli_set_charset($conn, "utf8");
                $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek WHERE ar >= " . $_POST['f_armin'] . " and ar <= " . $_POST['f_armax'] . ";";
                $s = $conn->query($select);
                $conn->close();
                if ($s->num_rows > 0) {
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
                    echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
                }
            } else {
                if ($_POST['f_nev'] <> "") {
                    $conn = MySQLServerConnecter();
                    mysqli_set_charset($conn, "utf8");
                    $select = "SELECT id, kep, termeknev, leiras, keszleten, ar FROM termekek WHERE termeknev like '%$_POST[f_nev]%' and ar >= " . $_POST['f_armin'] . " and ar <= " . $_POST['f_armax'] . ";";
                    $s = $conn->query($select);
                    $conn->close();
                    if ($s->num_rows > 0) {
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
    if ($_POST['f_armin'] == "") {
        $_POST['f_armin'] = 1;
    }
    if ($_POST['f_armax'] == "" || $_POST['f_armax'] == 1) {
		$_POST['f_armax'] = $_SESSION["maximum_ar"];
    }
    if ($_POST['f_nev'] == "" && $_POST['f_armin'] == 1 && $_POST['f_armax'] == $_SESSION['maximum_ar']) {
        $conn = MySQLServerConnecter();
        mysqli_set_charset($conn, "utf8");
        $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek";
        $s = $conn->query($select);
        $conn->close();
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
        if ($_POST['f_nev'] <> "") {
            $conn = MySQLServerConnecter();
            mysqli_set_charset($conn, "utf8");
            $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek WHERE termeknev like '%$_POST[f_nev]%'";
            $s = $conn->query($select);
            $conn->close();
            if ($s->num_rows > 0) {
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
                echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
            }
        } else {
            if ($_POST['f_nev'] == "") {
                $conn = MySQLServerConnecter();
                mysqli_set_charset($conn, "utf8");
                $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek WHERE ar >= " . $_POST['f_armin'] . " and ar <= " . $_POST['f_armax'] . ";";
                $s = $conn->query($select);
                $conn->close();
                if ($s->num_rows > 0) {
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
                    echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
                }
            } else {
                if ($_POST['f_nev'] <> "") {
                    $conn = MySQLServerConnecter();
                    mysqli_set_charset($conn, "utf8");
                    $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek WHERE termeknev like '%$_POST[f_nev]%' and ar >= " . $_POST['f_armin'] . " and ar <= " . $_POST['f_armax'] . ";";
                    $s = $conn->query($select);
                    $conn->close();
                    if ($s->num_rows > 0) {
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
                        echo "<h2>Nincs a szűrésnek megfelelő elem!</h2>";
                    }
                }
            }
        }
    }
}
echo "<html>";
echo '<form action="index.php" method="post">
	<input class="btn btn-default" type="submit" value="Vissza">
	</form>';
echo "</html>";
?>
</div>
</body>
</html>