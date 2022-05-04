import React, { useEffect, useState } from "react";
import { FunnelFill } from "react-bootstrap-icons";
import { Search } from "react-feather";
import { Link } from "react-router-dom";
import HomeTable from "./HomeTable";
import Multiselect from "multiselect-react-dropdown";

const ListHome = () => {
    const [query, setQuery] = useState("");

    const [search, setSearch] = useState("");
    const [users, setUsers] = useState("");

    const handleChangeSearch = (e) => {
        e.preventDefault();

    };

    return (
        <>
            <h4 className="fw-bold text-danger">My Assignment</h4>
            <HomeTable />
        </>
    );
};

export default ListHome;
