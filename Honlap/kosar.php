<html>
    <?php
    session_start();
    require_once 'dbconnect.php';
    ?>
	
    <script type="text/javascript">
	
		function updatelabel(qtylabel)
		{
			
			var price = "price_"+qtylabel;
			//price = price.concat(qtylabel);
			var darab = parseInt(document.getElementById(qtylabel).value);
			var ar = parseInt(document.getElementById(price).innerHTML);
			var oldval = parseInt(document.getElementById(qtylabel).defaultValue);
			var osszar= parseInt(document.getElementById("osszar").innerHTML);
				osszar = osszar - (oldval*ar);
				osszar = osszar + (darab*ar);
			document.getElementById(qtylabel).defaultValue=darab;

			document.getElementById("osszar").innerHTML= osszar;
		}
		

    </script>
	
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
	<script>
	$( function(){

    $(document).on("click", ".btn-del", function(){
		var r = confirm("Biztosan törlöd?");
			if (r==true)
			{
				var parent = $(this).closest("tr");		
				var id = $(parent).attr("val");
				var data = {idd: id};
					$.ajax({
					url: "deletefromarray.php",
					type: 'POST',
					dataType: 'json',
					data: data
					});
			window.location.href= "kosar.php";
			}
	   
	});
	});
	</script>
    <body>
        <form action=checkout.php method=post>
            <table>
                <thead>
                    <?php
					
                    if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {

                        echo '<tr>
                        <th>Kép:</th>
                        <th>Név:</th>
                        <th>Ár:</th>
                        <th>Darabszám:</th>
						<th>Törlés:</th>
                    </tr>
                </thead>
                <tbody>
                    A kosár tartalma:';

//unset($_SESSION['cart2']);
                        if (count($_POST) > 0)
                            $termekid = $_POST['id'];
                        else
                            $termekid = 0;
                        $conn = MySQLServerConnecter();
                        mysqli_set_charset($conn, "utf8");

                        if (!isset($_SESSION['cart2'])) {
                            $_SESSION['cart2'] = array();
                        }
						$bennevan = false;
						foreach ($_SESSION['cart2'] as $key => $value) {
							if ($value==$termekid)
							{
								$bennevan=true;
							}								
						}
						
                       /* if (!array_key_exists($termekid, $_SESSION['cart2'])) {
                            array_push($_SESSION['cart2'], $termekid);
                        }*/
						
						if ($bennevan==false)
						{
							array_push($_SESSION['cart2'], $termekid);
						}

                        $dbhelye = 0;
						$osszar=0;
                        foreach ($_SESSION['cart2'] as $key => $value) {
							
                            $query = "SELECT kep, termeknev,ar FROM termekek WHERE id=" . $value;
                            $result = mysqli_query($conn, $query) or die("Nem sikerült" . $query);

                            while (list($kep, $termeknev, $termekar) = mysqli_fetch_row($result)) {
                                echo '<tr val="'.$value.'">
				<td><img src=./image/' . $kep . ' alt=fenykep style=width:50px;height:50px;></td>
				<td>' . $termeknev . '</td>		
				<td id="price_'.$dbhelye.'">' . $termekar . '</td>
				<td>Darabszám:<input type="number" name='.$dbhelye.' id="' . $dbhelye . '" size="3" min="0" max="99" value="1"; onchange="updatelabel('.$dbhelye++.')"/></td>
				<td><button type="button" class="btn btn-del">X</button></td>
				</tr>';
							$osszar=$osszar+intval($termekar);
                            }
                        }
                        mysqli_close($conn);
                        //echo $dbhelye;
                        echo '
	 <tr></tr>
	 <tr>
	 <td colspan="2">Összár:</td>
	 <td id="osszar">'.$osszar.'</td>
	 <td>Ft</td>
	 </td>
	 </tr>
	 <tr>
	 <td colspan="4"><input type="button" onClick="parent.location=\'index.php\'" value="Vissza a vásárláshoz"></input>
	 <input type="submit" value="Megrendelem"></input></td>
	 </tr>
	 ';
                    } else {
                        echo '<meta http-equiv="refresh" content="0; URL=index.php">';
                    }
                    ?>
                    </tbody>
            </table>

        </form>
    </body>
</html>