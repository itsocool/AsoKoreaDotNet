var ax = document.getElementById("asoPosAX");

function CardVO() {
	this.billNo = "jsCardVO";
	this.businessNO = null;
	this.ownerName = null;
	this.companyName = null;
	this.greetings = null;

	this.cardItemVOList = null;
	this.approvalVOList = null;
}

function CardItemVO() {
	this.SN = null;
	this.salePrice = null;
	this.QTY = null;
}

function CardApprovalVO() {
	this.amount = null;
	this.halbu = null;
	this.gubun = null;
	this.org_auth_date = null;
	this.org_auth_no = null;
	this.is_confirm = null;
	this.confirmMSG = null;
}

function cardApprove(){

	var amount = 2000;
	var halbu  = "01";
	var gubun = 1;
	var org_auth_date = "";
	var org_auth_no = "";

	var vo = new CardVO();
	var flexApp = null;
	var callback = null;
	var item = new CardApprovalVO();
	
	vo = window["vo"];
	flexApp = window["flexApp"];
	callback = window["callback"];
	
	try {
		if(vo && vo["approvalVOList"]){
			item = vo.approvalVOList[vo.approvalVOList.length-1];
			amount = item.amount;
			halbu = item.halbu;
			gubun = item.gubun;
			org_auth_date = item.org_auth_date;
			org_auth_no = item.org_auth_no;
		}

		if(typeof(ax) === 'undefined' || ax === null){
	        alert('Unable to load ActiveX');
	        return;
	    }
		
		var recvVO = ax.CreditCardApprove(amount, halbu, gubun, org_auth_date, org_auth_no);
		
	} catch (e) {
		alert(e);
	}

	if(flexApp[callback]){
		flexApp[callback](vo);
		self.close();
	}
	
}

function test(){
	var ax = document.getElementById("asoPosAX");
	
	var amount = 9;
	var halbu  = "01";
	var gubun = 1;
	var org_auth_date = "";
	var org_auth_no = "";
	
	var vo = new CardVO();
	vo.billNo = "js_bill_no1";
	
	if(vo){
		alert("cardApprove:" + vo.billNo);
	}
	
	if(typeof(ax) === 'undefined' || ax === null){
        alert('Unable to load ActiveX');
        return;
    }
	
	var status = ax.Test(vo);
}

//function eh(s){
//	alert("eh:" + s);
//}
//
//
//function openCardPopup(appName, callback, vo) {
//
//	POS_POPUP_WINDOW = window.open("asopos.html", vo.billNo, "resize=yes,width=1000,height=700");
//	POS_POPUP_WINDOW.CardVO = vo;
//	POS_POPUP_WINDOW.appName = appName;
//	POS_POPUP_WINDOW.callback = callback;
//	
//	var input = POS_POPUP_WINDOW.document.createElement("input");
//	input.setAttribute("id", "billNo");
//	input.setAttribute("value", vo.billNo);
//	var s = POS_POPUP_WINDOW.document.getElementsByTagName("body")[0];
//	s.parentNode.insertBefore(input, s);
//	
//	var w = POS_POPUP_WINDOW;
//	
//	if(w != null && w != undefined){
//		var ax = document.getElementById(appName);
//		var func = ax[callback];
//		func(vo);
//	}else{
//		alert("no popup");
//	}
//	
//	return w;	
//}
//
//function CreditCardApprove(appName, callback, vo){
//	var amount = vo.amount;
//	var halbu = vo.amount;
//	var gubun = vo.amount;
//	var org_auth_date = vo.amount;
//	var org_auth_no = vo.amount;
//	var w = POS_POPUP_WINDOW;
//	
//	if(w != null && w != undefined){
//		var ax = w.document.getElementById(appName);
//		var func = ax[callback];
//		func(amount, halbu, gubun, org_auth_date, org_auth_no);
//	}else{
//		alert("no popup");
//	}
//}
