<html>
    <?php
    session_start();
    require_once 'dbconnect.php';
    ?>
	<head>
		<meta charset="UTF-8">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
		<title>Kosár</title>
	</head>
		
    <script type="text/javascript">
		//A rendelt termékek árának összeadására és label-be írására
        function updatelabel(qtylabel)
        {
			//A megfelelő termék árának kinyerése
            var price = "price_" + qtylabel;
            //price = price.concat(qtylabel);
			//Adatok beolvasása
            var darab = parseInt(document.getElementById(qtylabel).value);
            var ar = parseInt(document.getElementById(price).innerHTML);
			//Az oldval tárolja azt, hogy a darabszám módosítás előtt hány db volt belőle. Így újralehet számolni az összértéket.
            var oldval = parseInt(document.getElementById(qtylabel).defaultValue);
            var osszar = parseInt(document.getElementById("osszar").innerHTML);
			//Az összárból kivonjuk a régi darabszámot a termék árával összeszorozva, majd hozzáadjuk az összárhoz az új darabszámot az árral szorozva.
            osszar = osszar - (oldval * ar);
            osszar = osszar + (darab * ar);
            document.getElementById(qtylabel).defaultValue = darab;
			//Beírjuk a labelbe az új összárat.
            document.getElementById("osszar").innerHTML = osszar;
        }


    </script>

	
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script>
    $(function () {
		//Termék törlése a kosárból funkció
        $(document).on("click", ".btn-del", function () {
			//Megerősítő alert a törlésre
            var r = confirm("Biztosan törlöd?");
			//Ha igen gomb, akkor:
            if (r == true)
            {
				//Megkeresi a gombhoz eső legközelebbi sort(<tr>). (amiben van a gomb)
                var parent = $(this).closest("tr");
				//A termék id-je tárolva van a tr tag 'val' attributumában. Ezt nyeri ki:
                var id = $(parent).attr("val");
                var data = {idd: id};
				//Ajax segítségével JSON-be továbbküldi az ID-t a delefromarray.php-nak.
                $.ajax({
                    url: "deletefromarray.php",
                    type: 'POST',
                    dataType: 'json',
                    data: data
                });
				//Törlés után térjen vissza a kosárhoz.
                window.location.href = "kosar.php";
            }

        });
    });
    </script>
    <body>
	<div class="container">
		<div class="page-header">
            <h1>Kosár</h1>
        </div>
        <form action=checkout.php method=post>
		<div class="col-sm-6">
            <table class='table table-striped'>
                <thead>
                    <?php
                    //ha bevan jelentkezve a felhasználó
                    if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
						//A kosár táblázatának betöltése és feltöltése az adatokkal
                        echo '<tr>
                        <th>Kép:</th>
                        <th>Név:</th>
                        <th>Ár:</th>
                        <th>Darabszám:</th>
						<th>Törlés:</th>
                    </tr>
                </thead>
                <tbody>
                    <h3>A kosár tartalma:</h3>';
						//Ha kapott a kód termék ID-t, akkor mentse el a $ermekid-ba.
                        if (count($_POST) > 0)
                            $termekid = $_POST['id'];
                        else
                            $termekid = 0;
                        $conn = MySQLServerConnecter();
                        mysqli_set_charset($conn, "utf8");
						//Ha a kosár elemeinek nincs még létrehozva a globális tömb, akkor hozzon létre egyet cart2 néven.
                        if (!isset($_SESSION['cart2'])) {
                            $_SESSION['cart2'] = array();
                        }
                        $bennevan = false; //bool változó arra, hogy a hozzáadandó termék benne van-e a tömbben.
						//Bejárjuk a cart2 tömböt.
                        foreach ($_SESSION['cart2'] as $key => $value) {                           
							if ($value == $termekid) { //Ha a hozzáadandó termék benne van már a tömbben, akkor állítsa a $bennevan-t true-ra.
                                $bennevan = true;
                            }
                        }
						//Ha eddig nem szerepelt a termék a kosárban, akkor adja hozzá.
                        if ($bennevan == false) {
                            array_push($_SESSION['cart2'], $termekid);
                        }
						
                        $dbhelye = 0; //melyik sorszám
                        $osszar = 0; //termékek összára.
						//Lekérdezzük a cart2 tömbben lévő id-k alapján az összes belehelyezett termék adatait.
                        foreach ($_SESSION['cart2'] as $key => $value) {
					
                            $query = "SELECT kep, termeknev,ar FROM termekek WHERE id=" . $value;
                            $result = mysqli_query($conn, $query) or die("Nem sikerült letölteni az adatokat. Hiba: " . $query);

                            while (list($kep, $termeknev, $termekar) = mysqli_fetch_row($result)) {
								//Betöltjük az adatokat a táblázatba
                                echo '<tr val="' . $value . '">'. //A sor val attributumába elmentjük a termék id-ját.
								'<td><img src=./image/' . $kep . ' alt=fenykep style=width:50px;height:50px;></td>'. //Kép betöltése
								'<td>' . $termeknev . '</td>'.		//Terméknév beírása
								'<td id="price_' . $dbhelye . '">' . $termekar . '</td>.'. //a termékár td elem id-ja legyen a price_[kosár sorszám].
								//A darabszám neve és id-ja legyen a kosár termékének sorszáma. Így majd lehet hivatkozni rá a checkout.php-ban. Változásakor frissítse az összárat is. (onchange=updatelabel)
								'<td>Darabszám:<input type="number" name=' . $dbhelye . ' id="' . $dbhelye . '" size="3" min="0" max="99" value="1"; onchange="updatelabel(' . $dbhelye++ . ')"/></td>'. 
								'<td><button type="button" class="btn btn-del">X</button></td>'. //Törlés gomb
								'</tr>';
                                $osszar = $osszar + intval($termekar); //Az összárat növeljük a termék árával.
                            }
                        }
                        mysqli_close($conn); //Csatlakozás lezárása.
                        //Gombok és az összár kiíratása
						echo '
						<tr></tr>
						<tr>
							<td colspan="2">Összár:</td>
							<td id="osszar">' . $osszar . '</td>
							<td>Ft</td>
						</tr>
						<tr>
						<td colspan="4">
							<input type="button" class="form-control " onClick="parent.location=\'index.php\'" value="Vissza a vásárláshoz"></input><br>
							<input  type="submit" class="form-control " value="Megrendelem"></input>
						</td>
						</tr>
						';
                    }
					else 
					{
                     //nincs bejelentkezve visszadobja az index.php-re
                     echo '<meta http-equiv="refresh" content="0; URL=index.php">';
                    }
                    ?>
                    </tbody>
            </table>
		</div>
        </form>
		</div>
    </body>
</html>