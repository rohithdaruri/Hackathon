import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Records } from './components/Records';
import { Documentation } from './components/Documentation';
import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route exact path='/home' component={Home} />
                <Route path='/records' component={Records} />
                <Route path='/documentation' component={Documentation} />
            </Layout>
        );
    }
}
