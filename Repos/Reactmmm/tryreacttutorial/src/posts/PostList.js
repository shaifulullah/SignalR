import React, { Component } from 'react'
import PostData from '../data/posts.json'

import PostDetail from './PostDetail'
export default class PostList extends Component {
    render() {
        return (
            <div>
                <h1>Hello There</h1>
                {PostData.map((item, index) => {
                    return <PostDetail post={item} key={`post-list-key ${index}`} />
                    // return <div>
                    //     <h1>{postDetails.title}</h1>
                    //     <p>{postDetails.content}</p>
                    // </div>
                })}
            </div>
        )
    }
}
