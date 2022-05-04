import { AppBar, Toolbar, Typography } from '@mui/material';
import HeaderDropdown from '../../components/Dropdown/HeaderDropdown';
import HeaderBreadcrumbs from "./Breadcrumbs";
import useScrollTrigger from '@mui/material/useScrollTrigger';
import Slide from '@mui/material/Slide';


function HideOnScroll({ children }) {
    const trigger = useScrollTrigger()
    return (
      <Slide appear={false} direction={"down"} in={!trigger}>
        {children}
      </Slide>
    )
  }
export default function Header() {
    return (
    <HideOnScroll>
        <AppBar position ='static' sx={{mb:4}} style={{backgroundColor: "#CF2338"}}>
        <Toolbar sx={{display: 'flex', justifyContent:'space-between', alignItems: 'center'}}>
            <Typography>
                <HeaderBreadcrumbs/>
            </Typography>
            <HeaderDropdown/>
        </Toolbar>
        </AppBar>
    </HideOnScroll>
    )
}
