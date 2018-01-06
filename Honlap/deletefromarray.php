<?php
session_start();
//Ha a kód megkapta az idd változó tartalmát a kosar.php-tól:
if (isset($_REQUEST['idd']))
{
    //A globálisan tárolt cart2 tömbből törölje ki a kapott id-hez tartozó elemet
	$key=array_search($_REQUEST['idd'],$_SESSION['cart2']); //Megkeresi a tömbben az ID-t
    if($key!==false) //Ha megtalálta, akkor
    unset($_SESSION['cart2'][$key]); //törölje ki a tömb elemet
    $_SESSION["cart2"] = array_values($_SESSION["cart2"]);	//Tömb újrarendezése
	
} 
?>
