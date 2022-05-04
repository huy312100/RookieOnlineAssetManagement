
import { Redirect, Route } from "react-router"
import { toast } from "react-toastify"
import { useAppSelector } from "../store/configureStore"

export default function PrivateRoute({ component: Component, roles, ...rest }) {
  const { user } = useAppSelector(state => state.account)
  return (
    <Route
      {...rest}
      render={props => {
        if (!user) {
          return (
            <Redirect
              to={{ pathname: "/login", state: { from: props.location } }}
            />
          )
        }

        if (roles && !roles?.some(r => user.roles?.includes(r))) {
          toast.error("Not authorised to access this area")
          return <Redirect to={{ pathname: "/catalog" }} />
        }

        return <Component {...props} />
      }}
    />
  )
}