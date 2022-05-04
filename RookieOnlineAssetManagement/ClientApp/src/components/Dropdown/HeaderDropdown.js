import axios from "axios";
import React, { useState, useEffect } from 'react';
import { Modal , Box, Typography, Divider,TextField,InputAdornment,IconButton,Menu,MenuItem,Button } from "@mui/material";
import {Visibility,VisibilityOff} from "@mui/icons-material";
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';

function HeaderDropdown() {
  // State for change password
  const [openModalChangePassword, setOpenModalChangePassword] = useState(false);
  const [isIncorrectPassword, setIsIncorrectPassword] = useState(false);
  const [isDisable, setIsDisable] = useState(true);
  const [isDisableChangeFirstTime,setDisableChangeFirstTime] =useState(true);
  const [passwordChange, setPasswordChange] = useState({
    oldPassword: "",
    newPassword: "",
  });
  const [showOldPassword,setOldShowPassword] = useState(false);
  const [showNewPassword,setNewShowPassword] = useState(false);
  const [openModalChangePasswordSuccess, setOpenModalChangePasswordSuccess] =
  useState(false);

  // State for change password first time
  const [userData, setUserData] = useState({});
  const [openModalChangePasswordFirstTime, setOpenModalChangePasswordFirstTime] = useState(false);
  const [showChangePassFirsttime, setShowChangePassFirsttime ] = useState(false);
  const [passwordFirstTimeChange, setPasswordFirstTimeChange] = useState("");

  // State for logout
  const [openModalLogout,setOpenModalLogout] = useState(false);

  const toggleModalChangePassword = () => {
    setOpenModalChangePassword(!openModalChangePassword);
    setPasswordChange({ oldPassword: "", newPassword: "" });
    setIsIncorrectPassword(false);
  };
  
  const [anchorEl, setAnchorEl] = React.useState(null);
  const open = Boolean(anchorEl);
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleChangePasswordMenuSelect = () => {
    toggleModalChangePassword();
    setAnchorEl(null);
    }

  const handleLogoutModal = () => {
      setAnchorEl(null);
      setOpenModalLogout(true);
  }

  const handleLogout=()=>{
    axios.get("/api/Users/Logout").then((response)=>{
      window.location.href =
      "/Identity/Account/Login?returnUrl=/";
      setOpenModalLogout(false);
    });
  }

  const handleChangePasswordOnClick = () => {
    axios.put("/api/users/changepassword", {
      userId: userData.id,
      passwordOld: passwordChange.oldPassword,
      passwordNew: passwordChange.newPassword,
    })
    .then((response) => {
      
      toggleModalChangePassword();
      setOpenModalChangePasswordSuccess(true);
    })
    .catch((error) => {
      if (error.message !== "") {
        console.log(error);
        setIsIncorrectPassword(true);
      }
    });
  }

  const handleChangePasswordFirstTimeOnClick =()=>{
    axios.put("/api/users/changepasswordfirsttime", {
      userId: userData.id,
      passwordNew: passwordFirstTimeChange,
    })
    .then((response) => {
      setOpenModalChangePasswordFirstTime(!openModalChangePasswordFirstTime);
      setPasswordFirstTimeChange("");
    })
    .catch((error) => {
    });
  }

  // useEffect for change password
  useEffect(() => {
    if (passwordChange.newPassword !== "" && passwordChange.oldPassword !== "" ) {
      setIsDisable(false);
      if(passwordChange.newPassword === passwordChange.oldPassword){
        setIsDisable(true);
      }
      if(passwordChange.newPassword.length < 8 || passwordChange.newPassword.length > 50||/\s/.test(passwordChange.newPassword)){
        setIsDisable(true);
      }
      if(/\s/.test(passwordChange.oldPassword)){
        setIsDisable(true);
      }
    }
    else {
      setIsDisable(true);
    }
  }, [passwordChange]);

  //useeffect for change password first time
  useEffect(() => {
    if (passwordFirstTimeChange.length < 8 || passwordFirstTimeChange.length>50 || /\s/.test(passwordFirstTimeChange)) {
      setDisableChangeFirstTime(true);
    }
    else {
      setDisableChangeFirstTime(false);
    }
  }, [passwordFirstTimeChange]);

  //useeffect for get data login
  useEffect(() => {
    axios.get("/api/users/GetUserLogin").then((response) => {
      setUserData(response.data);
      setOpenModalChangePasswordFirstTime(response.data.isNew);
    });
  },[]);

  return (
    <div>
      <Button
        id="basic-button"
        style={{color:"white",textTransform:"none"}}
        aria-controls={open ? 'basic-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={open ? 'true' : undefined}
        onClick={handleClick}
      >
        {userData.userName}
        <ArrowDropDownIcon></ArrowDropDownIcon>
      </Button>
      <Menu
        id="basic-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        MenuListProps={{
          'aria-labelledby': 'basic-button',
        }}
      >
        <MenuItem onClick={handleChangePasswordMenuSelect}>Change password</MenuItem>
        <MenuItem onClick={handleLogoutModal}>Logout</MenuItem>
      </Menu>
      
      {/* Change password first time */}
      <Modal
        open={openModalChangePasswordFirstTime}
        // onClose={()=>setOpenModalChangePasswordFirstTime(false)}
      >
          <Box sx={[stylesChangePassword,{width:380}]} >
            <Typography
              id="modal-modal-title"
              variant="h6"
              component="h2"
              className="ChangePasswordTypography"
              style={{paddingLeft: "33px",color: "red", backgroundColor:"#eff1f5", height:50, borderRadius:5, paddingTop: "10px"}}
            >
              <h6><b>Change password</b></h6>
            </Typography>

            <Divider style={{}} />
  
            <form style={{ paddingLeft: "33px",paddingRight: "5px", paddingTop: "20px",paddingBottom: "30px",backgroundColor:"#fafcfc",borderRadius:5 }}>
            <Box style= {{backgroundColor:"#fafcfc", height:120, borderRadius:5}}>
            <div style={{paddingRight: "2px", paddingTop: "20px", paddingbottom: "40px", height: 40 }} >        
             <p style={{fontSize: "14px",marginBottom: '20px'}}>This is first time you logged in. <br/> You have to change your password to continue  </p>
                <label
                  for="fname"
                  style={{fontSize: "14px",marginRight : "22px",display:"inline-block"}}
                >
                 New Password
                </label>
                  <TextField
                    type= {showChangePassFirsttime ? "text" : "password"} 
                    size="small"
                    value={passwordFirstTimeChange}
                    label="Enter password"
                    onChange={(e) => setPasswordFirstTimeChange(e.target.value)             
                    }
                    inputProps={{
                      style: {
                        background: "white",
                        height: "10px",
                        width: "122px",
                        display:"inline-block", 
                      },
                      maxLength: 50
                    }}
                    InputLabelProps={{
                      style: { fontSize: "13px"},
                    }}
                    InputProps={{
                      endAdornment: ( 
                        <InputAdornment position="end" >
                          <IconButton
                            onClick={() => setShowChangePassFirsttime(!showChangePassFirsttime)}
                            disableRipple={true} size="small"
                          >
                            {showChangePassFirsttime ? <Visibility style={{ fontSize: "12px"}}/> : <VisibilityOff style={{ fontSize: "12px"}}/>}
                          </IconButton>
                        </InputAdornment>
                      )
                    }}
                  />   
                </div>
             </Box>    

                     
             {/\s/.test(passwordFirstTimeChange) ? <div style={{ color: "red" , fontSize: "12px",marginLeft:"100px" }}>New password does not contain spaces</div> : ""}

              <div style={{ marginLeft: "69%", marginTop: "15px" }}>
                <Button
                  variant="primary"
                  size="small"
                  onClick={handleChangePasswordFirstTimeOnClick}
                  style={{
                    backgroundColor: "red",
                    textSize: "10px",
                    padding: "2px",
                    marginRight: "20px",
                  }}
                  disabled={isDisableChangeFirstTime}
                >
                  <span
                    style={{
                      fontSize: "12px",
                      color: "white",
                      textTransform: "capitalize",
                      fontWeight: "bold",
                    }}
                  >
                    Save
                  </span>
                </Button>
              </div>
            </form>
          </Box>
        </Modal>


      {/* Modal changepassword  */}
      <Modal
        open={openModalChangePassword}
        onClose={toggleModalChangePassword}
        >
          <Box sx={stylesChangePassword} >
            <Typography
              id="modal-modal-title"
              variant="h6"
              component="h2"
              className="ChangePasswordTypography"
              style={{paddingLeft: "33px",color: "red", backgroundColor:"#eff1f5", height:40, borderRadius:5, paddingTop: "10px"}}
            >
              <h6><b>Change Password</b></h6>
            </Typography>

            <Divider style={{}} />
  
            <form style={{ paddingLeft: "33px", paddingTop: "20px",paddingBottom: "30px",backgroundColor:"#fafcfc",borderRadius:5 }}>
  
                <label
                  for="fname"
                  // className="LabemalOldPass"
                  style={{fontSize: "14px",marginRight : "10px",display:"inline-block"}}
                >
                  Old Password
                </label>
                  <TextField
                    type= {showOldPassword ? "text" : "password"} 
                    size="small"
                    value={passwordChange.oldPassword}
                    label="Enter old password"
                    onChange={(e) =>
                      setPasswordChange({
                        ...passwordChange,
                        oldPassword: e.target.value,
                      })
                    }
                    inputProps={{
                      style: {
                        background: "white",
                        height: "10px",
                        width: "122px",
                        display:"inline-block"
                        // marginRight: 0,
                      },
                    }}
                    InputLabelProps={{
                      style: { fontSize: "13px"},
                    }}
                    InputProps={{
                      endAdornment: ( 
                        <InputAdornment position="end" >
                          <IconButton
                            onClick={() => setOldShowPassword(!showOldPassword)}
                            disableRipple={true} size="small"
                          >
                            {showOldPassword ? <Visibility style={{ fontSize: "12px"}}/> : <VisibilityOff style={{ fontSize: "12px"}}/>}
                          </IconButton>
                        </InputAdornment>
                      )
                    }}
                  /> 

                  <div style={{ color: isIncorrectPassword ? "red" : "transparent", fontSize: "12px",marginLeft:"100px",marginBottom:"10px"}}>Password is incorrect</div> 
  
              <label for="lname" style={{ fontSize: "14px",marginRight: "5px",display:"inline-block" }}>
                New Password{" "}
              </label>
  
              <TextField
                type= {showNewPassword ? "text" : "password"} 
                size="small"
                style={{marginBottom:"10px"}}
                label="Enter new password"
                value={passwordChange.newPassword}
                onChange={(e) =>
                  setPasswordChange({
                    ...passwordChange,
                    newPassword: e.target.value,
                  })
                }
                inputProps={{
                  style: {
                    background: "white",
                    height: "10px",
                    width: "122px",
                    display:"inline-block"
                    // marginRight: 0,
                  },
                  maxLength: 50
                }}
                InputLabelProps={{
                  style: { fontSize: "13px"},
                }}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end" >
                      <IconButton
                        onClick={() => setNewShowPassword(!showNewPassword)}
                        disableRipple={true} size="small"
                      >
                        {showNewPassword ? <Visibility style={{ fontSize: "12px"}}/> : <VisibilityOff style={{ fontSize: "12px"}}/>}
                      </IconButton>
                    </InputAdornment>
                  )
                }}
             /> 
  
              {/\s/.test(passwordChange.newPassword) ? <div style={{ color: "red" , fontSize: "12px",marginLeft:"100px" }}>New password does not contain spaces</div> : ""}
              {passwordChange.oldPassword === passwordChange.newPassword && passwordChange.oldPassword.length !== 0 && passwordChange.newPassword.length
              ? <div style={{ color: "red" , fontSize: "12px",marginLeft:"100px" }}>The new password matches the old password</div> : "" }
              
              <div style={{ marginLeft: "40%", marginTop: "15px" }}>
                <Button
                  variant="primary"
                  size="small"
                  onClick={handleChangePasswordOnClick}
                  style={{
                    backgroundColor: isDisable ? "grey" : "red",
                    textSize: "10px",
                    padding: "2px",
                    marginRight: "20px",
                  }}
                  disabled={isDisable}
                >
                  <span
                    style={{
                      fontSize: "12px",
                      color: "white",
                      textTransform: "capitalize",
                      fontWeight: "bold",
                    }}
                  >
                    Save
                  </span>
                </Button>
  
                <Button
                  variant="primary"
                  size="small"
                  onClick={toggleModalChangePassword}
                  style={{
                    padding: "2px",
                    border: "1px solid #808080",
                    borderRadius: "5px",
                  }}
                >
                  <span
                    style={{
                      fontSize: "12px",
                      color: "grey",
                      textTransform: "capitalize",
                    }}
                  >
                    Cancel
                  </span>
                </Button>
              </div>
            </form>

          </Box>
        </Modal>

        {/* Open successfully changepassword */}
        <Modal
          open={openModalChangePasswordSuccess}
          onClose={()=>setOpenModalChangePasswordSuccess(false)}
   
        >
          <Box sx={[stylesChangePassword,{width:260, height: 160}]}>
            <Typography
              id="modal-modal-title"
              variant="h6"
              component="h2"
              style={{ paddingLeft: "40px", color: "red",paddingTop: "10px",paddingbottom: "20px"  }}
            >
               <h5><b>Change Password</b></h5>
            </Typography>
            <Divider style={{}} />
            <Box style= {{backgroundColor:"#fafcfc", height:120, borderRadius:5}}>
            <div style={{paddingLeft: "40px",paddingTop: "20px", paddingbottom: "40px", height: 40 }} >        
             <p style={{fontSize: "12px",marginBottom: '20px'}}>Your password has been changed successfully! </p>
              <Button
                  variant="primary"
                  size="small"
                  onClick={()=>setOpenModalChangePasswordSuccess(false)}
                  style={{
                    padding: "1px",
                    border: "1px solid #808080",
                    borderRadius: "5px",
                    position: 'absolute',
                    right: 45,
                  }}
                >
                  <span
                    style={{
                      fontSize: "12px",
                      color: "grey",
                      textTransform: "capitalize",
                      
                    }}
                  >
                    Close
                  </span>
              </Button>         
            </div>
            </Box>
          </Box>
        </Modal>

        {/* Open logout modal */}
        <Modal 
          open={openModalLogout}
          onClose={()=>setOpenModalLogout(false)}
   
        >
          <Box sx={[stylesChangePassword,{width:260, height: 160}]}>
            <Box style= {{backgroundColor:"#eff1f5", height:40, borderRadius:5}} >
              <Typography
                id="modal-modal-title"
                variant="h6"
                component="h2"
                style={{ paddingLeft: "40px", color: "red",paddingTop: "10px",paddingbottom: "20px"  }}
              >
                <h5><b>Are you sure?</b></h5>
              </Typography>
            </Box>
            
            <Divider style={{}} />
            <Box style= {{backgroundColor:"#fafcfc", height:40, borderRadius:5}} >
            <div style={{paddingLeft: "40px",paddingTop: "20px", paddingbottom: "40px", height: 20 }}>
              <p style={{fontSize: "12px"}}> 
                <h6>Do you want to log out?</h6>
              </p>

              <Button
                  variant="primary"
                  size="medium"
                  onClick={handleLogout}
                  style={{
                    padding: "3px",
                    borderRadius: "5px",
                    backgroundColor: "red",
                    marginRight: "20px",
                    paddingTop: "5px",
                    textTransform: 'capitalize'
                  }}
                >
                  <span
                    style={{
                      fontSize: "12px",
                      color: "white",
                    }}
                  >
                    Log out
                  </span>
              </Button>

              <Button 
                  variant="primary"
                  size="medium"
                  onClick={()=>setOpenModalLogout(false)}
                  style={{
                    padding: "2px",
                    border: "1px solid #808080",
                    borderRadius: "5px",
                    paddingTop: "5px",
                    textTransform: 'capitalize'
                  }}
                >
                  <span
                    style={{
                      fontSize: "12px",
                      color: "grey",

                    }}
                  >
                    Cancel
                  </span>
              </Button>
            </div>                   
            </Box>

          </Box>
        </Modal>
    </div>
  );
}

const stylesChangePassword =  {
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    width: 350,
    bgcolor: "background.paper",
    border: "1px solid",
    boxShadow: 24,
    borderRadius:2.5
};

export default HeaderDropdown;